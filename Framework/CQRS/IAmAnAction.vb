﻿Imports System.Security.Principal

Namespace CQRS

    ''' <summary>
    ''' Values of the interface is declared as functions to avoid serializing.. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IAmAnAction
        Inherits IActionBase

        Function User() As IPrincipal
        Function Guid() As Guid
        Function TimeStamp() As Long
        Function EndTimeStamp() As Long
        Sub ActionComplete()
        Sub HandlerStart()
        Function HandlerStartTimeStamp() As Long


    End Interface
End Namespace
