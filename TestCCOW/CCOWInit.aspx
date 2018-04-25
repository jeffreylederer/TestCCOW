<%@ Page Language="c#" Inherits="CCOWTest.CCOWInit" Codebehind="CCOWInit.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Default</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />

    <script language="JavaScript" type="text/javascript">
        function startup() {
            try {
                cmUrl = WebLocator.Run();
                if (cmUrl == null || cmUrl == "") {
                    self.location.replace("Death.aspx?a=0");
                }
                else {
                    url = "GetContextPage.aspx";
                    // append CM URL as GET parameter (URL encoded).
                    url = url + "?contextManagerUrl=" + escape(cmUrl);
                    window.status = "refreshing...";
                    self.location.replace(url);
                }
            }
            catch (e) {
                self.location.replace("Death.aspx?a=" + e.message);
            }
        }
    </script>

</head>
<body>
    <div align="center">
        <h3>
            Contacting Vergence Appliance...</h3>
        <object id="WebLocator" codebase="/Applets/WebXContextlets.cab#version=3,3,1,8907"
            height="1" width="1" classid="CLSID:EE986640-0821-4482-B4A3-C41EB8A18597" viewastext>
            <param name="contextParticipantUrl" value="<%= ContextParticipantUrl %>" />
            <param name="debug" value="debug" />
        </object>
        <form runat="server" id="form1">
        </form>
</body>

<script language="JavaScript" type="text/javascript">
    startup();
</script>

</html>