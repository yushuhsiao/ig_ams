var net = require("net"),
domains = ["*:*"]; 

net.createServer(
    function(socket)
    {
        /**
        * Start the flash policy file
        */
        socket.write("<?xml version=\"1.0\"?>");
        socket.write("<!DOCTYPE cross-domain-policy SYSTEM \"http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd\">\n");
        socket.write("<cross-domain-policy>\n");
    
        /**
        * Iterating over the domains array
        */
        domains.forEach(
            function(domain)
            {
                /**
                * Break the domain into domain and port values
                */
                var parts = domain.split(':');
            
                /**
                * Write them to the socket
                */
                socket.write("<allow-access-from domain=\""+parts[0]+"\"to-ports=\""+(parts[1]||'80')+"\"/>\n");
            }
        );
    
        /**
        * And we're done
        */
        socket.write("</cross-domain-policy>\n");
        require("sys").log("Wrote policy file.");
        socket.end();
    }
).listen(843);