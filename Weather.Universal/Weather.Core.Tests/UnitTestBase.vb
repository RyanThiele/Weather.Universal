
<TestClass()> Public MustInherit Class UnitTestBase

    Public Property TestContext As TestContext


    Protected Function CreateMessageBus() As IMessageBus
        Return New Fakes.StubIMessageBus
    End Function

    Protected Function CreateDialogService() As Services.IDialogService
        Return New Services.Fakes.StubIDialogService
    End Function

    Protected Function CreateNavigationService() As Services.INavigationService
        Return New Services.Fakes.StubINavigationService
    End Function

End Class
