﻿
Imports LazyFramework.CQRS
Imports LazyFramework.CQRS.Command
Imports NUnit.Framework
Imports System.Security.Principal

<TestFixture> Public Class CommandHub
    
    <SetUp> Public Sub SetupFixture()
        Runtime.Context.Current = New Runtime.WinThread
        LazyFramework.ClassFactory.Clear()
        LazyFramework.ClassFactory.SetTypeInstance(Of IActionSecurity)(New TestSecurity)
    End Sub

    <TearDown> Public Sub TearDown()
        LazyFramework.ClassFactory.Clear()
    End Sub

    <Test> Public Sub CommandIsLogged()

        Handling.ExecuteCommand(New TestCommand)
        'Assert.IsTrue(_TestLogger.LogIsCalled)

    End Sub


    <Test> Public Sub CommandIsRunned()

        Handling.ExecuteCommand(New TestCommand)
        Handling.ExecuteCommand(New anotherCommand)
        Assert.IsTrue(HandleCommands.Found)

    End Sub


    <Test> Public Sub ExecptionIsHandledCorrectly()

        Assert.Throws(Of InnerException)(Sub() Handling.ExecuteCommand(New ExceptionIsThrownCommand))

    End Sub

    <Test> Public Sub ByRefparemSubIsCalled()
        Dim byrefCommand As ByrefCommand = New ByrefCommand
        Handling.ExecuteCommand(byrefCommand)

        Assert.IsTrue(byrefCommand.Called)

    End Sub
    
    <Test> Public Sub CommandIsMappedToName()
        Dim toTest As New CommandForA

        Assert.AreEqual(toTest.ActionName, Handling.CommandList(toTest.ActionName).FullName.Replace("."c, ""))

    End Sub


    <Test> Public Sub ReadOnlyPropertiesIsNotSerialized()

        Dim a As New CommandForA

        Dim res = Newtonsoft.Json.JsonConvert.SerializeObject(a)

        Debug.Print(res)

    End Sub

    

End Class


Public Class Entity
    Public Property A As Integer
End Class


Public MustInherit Class BaseCommand(Of T)
    Inherits CommandBase(Of T)


End Class

Public Class CommandForA
    Inherits BaseCommand(Of Entity)

    Public MyParam As Integer = 1

End Class

Public Class AnotherCommandForA
    Inherits CommandForA

End Class





Public Class CalculateKm
    Inherits CommandBase

    Public KmDrive As Integer
    Public DateDriven As DateTime

    Public Overrides ReadOnly Property ActionName As String
        Get
            Return "Kalkuler km sats"
        End Get
    End Property
End Class

Public Class TestSecurity
    Implements IActionSecurity
    
    Public Function EntityIsAvailableForUser(user As Security.Principal.IPrincipal, action As IAmAnAction, entity As Object) As Boolean Implements IActionSecurity.EntityIsAvailableForUser
        Return True
    End Function

    Public Function GetActionList(user As Security.Principal.IPrincipal, action As IActionBase, entity As Object) As List(Of IActionDescriptor) Implements IActionSecurity.GetActionList
        Return New List(Of IActionDescriptor)
    End Function

    Public Function UserCanRunThisAction(user As IPrincipal, c As IActionBase) As Boolean Implements IActionSecurity.UserCanRunThisAction
        Return True
    End Function

    Public Function UserCanRunThisAction(user As IPrincipal, action As IActionBase, entity As Object) As Boolean Implements IActionSecurity.UserCanRunThisAction
        Return True
    End Function
End Class


Public Class HandleCommands
    Implements IHandleCommand

    Public Shared Found As Boolean = False



    Public Shared Sub Handle(cmd As CalculateKm)

        Dim res As Single

        res = CType((cmd.KmDrive * 4.14), Single)

        'EventHandling.EventHub.Publish()


    End Sub

    Public Shared Sub Handle(command As TestCommand)
        Found = True
    End Sub

End Class

Public Class InnerException
    Inherits Exception

End Class

Public Class ExceptionIsThrownCommand
    Inherits CommandBase

    Public Overrides ReadOnly Property ActionName As String
        Get
            Return "Exception"
        End Get
    End Property
End Class

Public Class AnotherCommand
    Inherits CommandBase

    Public Overrides ReadOnly Property ActionName As String
        Get
            Return "jbjkbkjb"
        End Get
    End Property
End Class

Public Class ByrefCommand
    Inherits CommandBase

    Public Called As Boolean = False

    Public Overrides ReadOnly Property ActionName As String
        Get
            Return ""
        End Get
    End Property
End Class

Public Class TestCommand
    Inherits CommandBase

    Public Overrides ReadOnly Property ActionName As String
        Get
            Return "Name"
        End Get
    End Property

    Public Overrides Sub OnActionBegin()
        MyBase.OnActionBegin()
    End Sub

    Public Overrides Sub OnActionComplete()
        MyBase.OnActionComplete()
    End Sub

End Class


Public Class Another
    Implements IHandleCommand

    Public Shared Sub HandleSomethingElse(cmd As anotherCommand)
        Dim a = cmd.ActionName
    End Sub


    Public Shared IsCalled As Boolean = False

    Public Shared Sub ParamIsByRef(ByRef cmd As ByrefCommand)
        cmd.Called = True
    End Sub

    Public Shared Sub ExceptionIsThrown(cmd As ExceptionIsThrownCommand)

        Throw New InnerException

    End Sub

End Class
