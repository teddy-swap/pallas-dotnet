namespace pallas_dotnet_test;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var nodeClient = PallasDotnetRs.PallasDotnetRs.Connect("/home/rawriclark/.dmtr/tmp/tasteful-infusion-213dd4/mainnet-v135.socket");
        await Task.Delay(1000 * 5);
        PallasDotnetRs.PallasDotnetRs.Disconnect(nodeClient);
        Assert.True(nodeClient.clientPtr > 0);
    }
}