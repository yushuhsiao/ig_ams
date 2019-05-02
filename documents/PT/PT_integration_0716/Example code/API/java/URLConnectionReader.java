import java.net.*;
import java.security.KeyStore;
import java.io.*;

import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.KeyManagerFactory;
import javax.net.ssl.SSLContext;

public class URLConnectionReader {
    public static void main(String[] args) throws Exception
    {
    	String PTUrl = "https://kioskpublicapi.redhorse88.com/";
    	String loginname ="test01";
    	String password ="123456";
    	String kioskname ="PPLAYDEV";
    	String adminname = "PPLAYDEV";
    	
   		String requestUrl = PTUrl + "createPlayer/"+ loginname + "/" + kioskname + "/" + adminname + "/password/" + password;
    	
        URL pt = new URL(requestUrl);
        URLConnection yc = pt.openConnection();
        
        KeyStore ks = KeyStore.getInstance("PKCS12");
        File file = new File("C:/Documents and Settings/alex.DELL/workspace/PT_API/src/play.p12");
        FileInputStream fis = new FileInputStream(file);
        ks.load(fis, "0lfJl6Yj".toCharArray());
        KeyManagerFactory kmf = KeyManagerFactory.getInstance("SunX509");
        kmf.init(ks, "0lfJl6Yj".toCharArray());
        SSLContext sc = SSLContext.getInstance("TLS");
        sc.init(kmf.getKeyManagers(), null, null);
        
        
        yc.setRequestProperty("X_ENTITY_KEY","27f6733f33367660f21910bc73c43ad2a3bdd29f6e09daefa15432bbf27e1b976fcc0b982d69d0c211eb2c69f51a73ddf8d07b5746aca6df234e8aa467e8583e");
        //yc.setSSLSocketFactory(sc.getSocketFactory());
        ((HttpsURLConnection) yc).setSSLSocketFactory(sc.getSocketFactory());
        BufferedReader in = new BufferedReader(new InputStreamReader(
                                    yc.getInputStream()));
        String inputLine;
        while ((inputLine = in.readLine()) != null) 
            System.out.println(inputLine);
        in.close();
    }
    
}
