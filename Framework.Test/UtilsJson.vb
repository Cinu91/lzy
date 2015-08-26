﻿
Imports System.Runtime.CompilerServices
Imports LazyFramework.Utils.Json
Imports NUnit.Framework

Module StringExtension
    <Extension()> Public Sub Deserialize(obj As String)
        Dim i = 0
    End Sub
End Module


<TestFixture> Public Class UtilsJson

    <SetUp> Public Sub Setup()

    End Sub

    <Test> Public Sub IntegerIsWritten()

        Assert.AreEqual("{""ToTest"":1}", Writer.ObjectToString(New With {.ToTest = 1}))

    End Sub


    <Test(Description:="Testing encoding of strings"),
        TestCase("StandardText"),
        TestCase("ÆØÅ{"),
        TestCase("Some€"),
        TestCase("with " & Chr(&H22) & " "),
        TestCase("\\\///" & Chr(9)),
        TestCase("--" & Chr(&HC), Description:="Formfeed"),
        TestCase("--" & vbCrLf),
        TestCase(Nothing)
        > Public Sub TextIsWritten(input As String)

        Dim value = New With {.ToTest = input, .test = input}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(value), Writer.ObjectToString(value))

    End Sub

    <Test> Public Sub BooleanIsWritten()
        Dim o = New With {.ThisIsTrue = true,.ThisIsFalse = False}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))
    End Sub


    <Test,
        TestCase(1),
        TestCase(123467854684),
        TestCase(-1)> Public Sub IntegersIsWritten(input As Long)
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))
    End Sub

    <Test,
        TestCase(1),
        TestCase(1234),
        TestCase(Nothing)> Public Sub NullableIntegers(input As Integer?)

        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))

    End Sub

    <Test,
        TestCase(1.123),
        TestCase(684.7853),
        TestCase(-1.45644)> Public Sub DoubleIsWritten(input As Double)
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))
    End Sub

    <Test,
        TestCase(1.123),
        TestCase(684.7853),
        TestCase(-1.45644)> Public Sub DoubleIsWritten(input As Decimal)
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(New With {.ToTest = input}), Writer.ObjectToString(New With {.ToTest = input}))
    End Sub

    <Test> Public Sub MultilevelObjects()
        Dim o = New With {.ToTest = "", .Child = New With {.Name = "Test", .Year = 12}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))
    End Sub


    <Test> Public Sub MultilevelObjectsWithStructure()
        Dim o = New With {.ToTest = "", .Child = New Test With {.Name = "jklj", .Year = 12}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))
    End Sub

    <Test> Public Sub IntegerArray()
        Dim toWrite As Integer() = {1, 2, 3}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))

    End Sub


    <Test> Public Sub InheritedAttributesIsWrittenToText()
        Dim o As New Person2

        o.Addresse = "blbla"
        o.Name = "Mikael"

        Assert.AreEqual("{""Addresse"":""blbla"",""Name"":""Mikael"",""Year"":0,""Scores"":null}", Writer.ObjectToString(o))

    End Sub

    Public Class PetterJson
        Public Shared Function Format(val As String) As String
            Return "TEST CONFIG"
        End Function
    End Class

    '<Test> Public Sub DateTimeAttributesIsWrittenToText()
    '    Dim o As New ExcavationTripDateTime

    '    'Dim text = PetterJson.Deserialize(obj)
    '    'Dim text = PetterJson.Format("YYMMMDD").Deserialize(obj)

    '    o.StartDate = New DateTime(1999, 6, 1)
    '    o.EndDate = New DateTime(2000, 6, 1)

    '    Writer.Config.FormatDate(Function(value As Date) Format(value, "DD-MM-YY T:M:S ")).ObjectToString(o)

    '    Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))

    'End Sub

    <Test> Public Sub DateAttributesIsWrittenToText()
        Dim o As New ExcavationTripDate

        o.StartDate = New Date(1999, 6, 1)
        o.EndDate = New Date(2000, 6, 1)

        'Assert.AreEqual("{""StartDate"":01.06.1999 00:00:00,""EndDate"":01.06.2000 00:00:00}", Writer.ObjectToString(o))
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(o), Writer.ObjectToString(o))
    End Sub

    <Test> Public Sub StringArray()
        Dim toWrite As String() = {"abc", "æøå", ""}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))

    End Sub
    <Test> Public Sub ObjectArray()
        Dim toWrite As Object() = {New With {.Name = "sd"}, New With {.Age = 42}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))

    End Sub

    <Test> Public Sub WriteValueTypeArray()

        Dim toWrite = New With {.Values = {1, 2, 3, 4}}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))
    End Sub


    <Test> public sub Serializeguid

        Dim toWrite = New With {.g = New Guid("FE41254C-FFFC-4121-8345-7353C5D128DC")}
        Assert.AreEqual(Newtonsoft.Json.JsonConvert.SerializeObject(toWrite), Writer.ObjectToString(toWrite))
    End sub 


End Class


<TestFixture> Public Class TestParser
    <Test> Public Sub ParseSimpleObject()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter""}  ")
        Assert.AreEqual("Petter", p.Navn)
        'Assert.AreEqual(43, p.Alder)
    End Sub

    <Test> Public Sub ParseTextWithEscapeObject()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter\nGjermund\\""}")
        Assert.AreEqual("Petter" & vbCrLf & "Gjermund\", p.Navn)
        'Assert.AreEqual(43, p.Alder)
    End Sub

    <Test> Public Sub ParseComplexObject()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter"",""TestInfo"": {""Name"":""Nils""  }   }")
        Assert.AreEqual("Petter", p.Navn)
        Assert.AreEqual("Nils", p.TestInfo.Name)
        'Assert.AreEqual(43, p.Alder)
    End Sub

    <Test> Public Sub ParseSimpleObjectWithInteger()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter"",""Alder"":  42   }")
        Assert.AreEqual("Petter", p.Navn)
        Assert.AreEqual(42, p.Alder)
    End Sub

    <Test> Public Sub ConsumeJsonWithComments()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)(My.Resources.ConsumeJsonWithComments)
        Assert.AreEqual("Petter", p.Navn)
        Assert.AreEqual(42, p.Alder)
    End Sub

    <Test> Public Sub ParseSimpleObjectWithDouble()
        Dim p = Utils.Json.Reader.StringToObject(Of Person)("{""Navn"":""Petter"",""Speed"":  4.2,""Alder"":42  }")
        Assert.AreEqual("Petter", p.Navn)
        Assert.AreEqual(4.2, p.Speed)
    End Sub


    <Test> Public Sub ParseEmptyList()
        Dim p = Reader.StringToObject(Of List(Of Person))("[]")
        Assert.IsInstanceOf(Of List(Of Person))(p)
        Assert.AreEqual(0, p.Count)
    End Sub

    <Test> Public Sub ParseListOfOneSimpleObject()
        Dim p = Reader.StringToObject(Of List(Of Person))("[{""Navn"":""Petter"",""Speed"":  4.2,""Alder"":42  }]")
        Assert.AreEqual("Petter", p(0).Navn)
        Assert.AreEqual(4.2, p(0).Speed)
    End Sub

    <Test> Public Sub ParseListOfManySimpleObject()
        Dim p = Reader.StringToObject(Of List(Of Person))("[{""Navn"":""Petter"",""Speed"":  4.2,""Alder"":42  },{""Navn"":""Gjermund"",""Speed"":  4.0,""Alder"":40  }]")
        Assert.AreEqual("Gjermund", p(1).Navn)
        Assert.AreEqual(4.0, p(1).Speed)
    End Sub

    <Test> Public Sub ParseListOfManyValueTypes()
        Dim p = Reader.StringToObject(Of List(Of Integer))("[1,2,3]")
        Assert.AreEqual(1, p(0))
        Assert.AreEqual(2, p(1))
    End Sub

    <Test> Public Sub ParseListOfStrings()
        Dim p = Reader.StringToObject(Of List(Of String))("[""1"",""2"",""3""]")
        Assert.AreEqual("1", p(0))
        Assert.AreEqual("2", p(1))
    End Sub

    <Test> Public Sub ParseListAsPartOfObject()
        Dim p = Reader.StringToObject(Of Test)("{""Name"":""Petter"",""Scores"" : [""1"",""2"",""3""]}")
        Assert.AreEqual("Petter", p.Name)
        Assert.AreEqual("1", p.Scores(0))
    End Sub

    <Test> Public Sub ParseValueTypesIntoArray()
        Dim p = Reader.StringToObject(Of TestWithIntArray)("{""Name"":""Petter"",""Scores"" : [1,2,3]}")
        Assert.AreEqual(1, p.Scores(0))
    End Sub

    <Test> Public Sub ParseValueTypesDoubleIntoArray()
        Dim p = Reader.StringToObject(Of TestWithDArray)("{""Name"":""Petter"",""Scores"" : [1.2,2.34,3.12]}")
        Assert.AreEqual(1.2, p.Scores(0))
    End Sub
    
    <test> Public Sub  Readguid()
        Dim p = Reader.StringToObject(Of Holder(Of Guid))("{""Value"":""FE41254C-FFFC-4121-8345-7353C5D128DC""}")
        Assert.AreEqual(New Guid("FE41254C-FFFC-4121-8345-7353C5D128DC"), p.Value)


    End Sub

End Class


Public Class Holder(Of T)
    Public Value As T
End Class


Public Class TestWithDArray
    Public Name As String
    Public Year As Integer
    Public Scores As Double()
End Class

Public Class TestWithIntArray
    Public Name As String
    Public Year As Integer
    Public Scores As Integer()
End Class

Public Class Test
    Public Name As String
    Public Year As Integer
    Public Scores As List(Of String)
End Class


Public Class Person
    Public Navn As String
    Public Alder As Integer
    Public Speed As Double
    Public Barn As List(Of Person)
    Public TestInfo As Test
End Class


Public Class Person2
    Inherits Test

    Public Addresse As String

End Class

Public Class ExcavationTripDateTime
    Public StartDate As DateTime
    Public EndDate As DateTime
End Class
Public Class ExcavationTripDate
    Public StartDate As Date
    Public EndDate As Date
End Class
