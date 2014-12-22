﻿

Namespace CQRS.Transform
    Public Class EntityTransformerProvider
        Private Shared ReadOnly PadLock As New Object
        Private Shared _allTransformers As Dictionary(Of Type, ITransformerFactory)

        Private Shared ReadOnly Property AllTransformers As Dictionary(Of Type, ITransformerFactory)
            Get
                If _allTransformers Is Nothing Then
                    SyncLock PadLock
                        If _allTransformers Is Nothing Then
                            Dim temp As New Dictionary(Of Type, ITransformerFactory)
                            For Each t In TypeValidation.FindAllClassesOfTypeInApplication(GetType(ITransformerFactory))
                                If Not t.IsAbstract Then
                                    If t.BaseType.IsGenericType Then
                                        Dim key = t.BaseType.GetGenericArguments()(0)
                                        Dim value = Activator.CreateInstance(t)
                                        temp.Add(key, CType(value, ITransformerFactory))
                                    End If
                                End If
                            Next
                            _allTransformers = temp
                        End If
                    End SyncLock
                End If

                Return _allTransformers
            End Get
        End Property

        Private Shared ReadOnly DefaultFactory As New DefaultEntiyTransformerFactory

        Public Shared Function GetFactory(ByVal action As IAmAnAction) As ITransformerFactory
            Dim t = action.GetType
            While t IsNot Nothing
                If AllTransformers.ContainsKey(t) Then
                    Return AllTransformers(t)
                End If
                t = t.BaseType
            End While

            Return DefaultFactory


        End Function


        Public Class DefaultEntiyTransformerFactory
            Implements ITransformerFactory
            
            ReadOnly _Trans As New DoNothingWithTheEntityTransformer

            Public Function GetTransformer(action As IAmAnAction, ent As Object) As ITransformEntityToDto Implements ITransformerFactory.GetTransformer
                Return _Trans
            End Function

            Public Class DoNothingWithTheEntityTransformer
                Implements ITransformEntityToDto

                Public Function TransformEntity(ByVal ent As Object) As Object Implements ITransformEntityToDto.TransformEntity
                    Return ent
                End Function

                Public Property Action As IAmAnAction Implements ITransformEntityToDto.Action
            End Class


            Public Function SortingFunc() As Comparison(Of Object) Implements ITransformerFactory.SortingFunc
                Return Nothing
            End Function
        End Class

    End Class
End Namespace
