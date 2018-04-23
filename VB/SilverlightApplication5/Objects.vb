Imports Microsoft.VisualBasic
Imports System
Imports System.Net
Imports DevExpress.Xpo

Namespace SilverlightApplication5
	Public Class Person
		Inherits XPObject
		Private firstName_Renamed As String
		Public Property FirstName() As String
			Get
				Return firstName_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue(Of String)("FirstName", firstName_Renamed, value)
			End Set
		End Property
		Private lastName_Renamed As String
		Public Property LastName() As String
			Get
				Return lastName_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue(Of String)("LastName", lastName_Renamed, value)
			End Set
		End Property
		Private birthdate_Renamed As DateTime
		Public Property Birthdate() As DateTime
			Get
				Return birthdate_Renamed
			End Get
			Set(ByVal value As DateTime)
				SetPropertyValue(Of DateTime)("Birthdate", birthdate_Renamed, value)
			End Set
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
	End Class
End Namespace
