using System.Text;
using ProxyWorld.wit.imports.wasi.http.v0_2_0;
using ProxyWorld.wit.imports.wasi.io.v0_2_0;

namespace ProxyWorld.wit.exports.wasi.http.v0_2_0;

public class IncomingHandlerImpl : IIncomingHandler
{
    public static void Handle(ITypes.IncomingRequest request, ITypes.ResponseOutparam responseOut)
    {
        byte[] content = Encoding.ASCII.GetBytes("-- Demo.Wasi.Http --");

        var headers = new List<(string, byte[])> {
            ("Content-Type", Encoding.ASCII.GetBytes("text/plain")),
	        ("Content-Length", Encoding.ASCII.GetBytes(content.Length.ToString()))
	    };

        ITypes.OutgoingResponse response = new ITypes.OutgoingResponse(ITypes.Fields.FromList(headers));
	    ITypes.ResponseOutparam.Set(responseOut, Result<ITypes.OutgoingResponse, ITypes.ErrorCode>.Ok(response));
        
        ITypes.OutgoingBody responseBody = response.Body();
	    using (IStreams.OutputStream responseBodyStream = responseBody.Write())
        {
            responseBodyStream.BlockingWriteAndFlush(content);
        }
        ITypes.OutgoingBody.Finish(responseBody, null);
    }
}