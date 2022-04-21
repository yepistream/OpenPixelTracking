using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Emgu.CV; //TODO: Switch package to OpenCV, or check for an update, might just be using an older version.
namespace ActualShitDetection
{
    public class capturePixel
    {
        public static float outputX,outputY;

        private static bool isRunning = true;

        static Bitmap bitage; //TODO: Modify it so it takes pictures from the webcam.
        static Bitmap bitage2;
        
        static private VideoCapture cameraDevice = new VideoCapture();
        
        static private Color targetColor = Color.Black;
        //? Used only in independent & debuging programs.
        /*static void Main(string[] args)
        {
            firstCapture();
            //File.Create("DemoText.txt");
            Task.Run(() => {calulateDistance(bitage,bitage2, 0f);});
            while(isRunning){
                isRunning = Console.ReadLine() == "s";
            }
            
        }*/

        public void updateOutput(){
            calulateDistance(bitage,bitage2);
        }

        static bool trySamePixel(Bitmap img, Color color ,out int[] n, int colorSensitivity = 0){
            int[] z = new int[2];
            Color s = new Color(), d = new Color();
            float a,b;
            for(int y = 0; y < img.Height; y++){
                for(int x = 0; x < img.Width-1; x++){
                    //Console.WriteLine(img.GetPixel(x,y));
                    s = img.GetPixel(x,y);
                    d = img.GetPixel(x+1,y);
                    b = d.R+d.B+d.G+d.A;
                    a = s.R+s.G+s.B+s.A;
                    if(a > (color.A+color.G+color.R+color.B)-colorSensitivity && a < (color.A+color.G+color.R+color.B)+colorSensitivity && b != a){
                        //Console.WriteLine("Found color at: " + x + " " + y);
                        z[0] = x; z[1]=y;
                        n = z;
                        return true;
                    }
                }
            }
            //Console.WriteLine("Error 404 : No color found ");
            n = null;
            return false;
        }

        public static void calulateDistance(Bitmap cpture1, Bitmap cpture2,float debugThreadSleep = 0 ){ 
                int[] lastLocc = new int[2], currentLocc = new int[2]; //? ,distanceLocc = new int[2]; Reffer to upper comment
                if(trySamePixel(cpture1, Color.Black, out lastLocc, 100) && trySamePixel(cpture2,targetColor,out currentLocc, 100)){
                    outputX = currentLocc[0]-lastLocc[0];
                    outputY = currentLocc[1]-lastLocc[1];
                    /*Console.WriteLine("The distance between the first images object and the seceond: X:" + distanceLocc[0] + " Y:" + distanceLocc[1]);
                    // TODO: Add something alike "bitage2 = bitage;" for it replaces a new last position, and then it gets a new current position.
                    File.WriteAllText("DemoText.txt","X:" + distanceLocc[0] + " Y:" + distanceLocc[1]);*/ // * Reffer to upper comment
                    cpture1 = cpture2;
                    cpture2 = cameraDevice.QueryFrame().ToBitmap();
                }
                //Thread.Sleep((int)(debugThreadSleep*1000)); //debugThreadSleep is in seceonds, thats why we * 1000 to convert it to secs instead of milisecs.
                if(isRunning){
                    Console.Clear();
                    calulateDistance(cpture1,cpture2,debugThreadSleep);
                }
        }

        public static void firstCapture(){
            do{
            Console.WriteLine("Press Enter when ready!");
            Console.ReadLine();
            
            bitage = cameraDevice.QueryFrame().ToBitmap();
            //bitage.Save(@"C:\Users\kazim\Desktop\A VR Thin\Image test\ControlledImageTesting\ActualShit\ActualShitDetection\TestImage.png", ImageFormat.Png); Reffer to upper comment
            //Console.WriteLine("Press Enter when ready to capture the seceond picture");
            //Console.ReadLine();
            bitage2 = cameraDevice.QueryFrame().ToBitmap();
            //bitage2.Save(@"C:\Users\kazim\Desktop\A VR Thin\Image test\ControlledImageTesting\ActualShit\ActualShitDetection\TestImage2.png", ImageFormat.Png); Reffer to upper comment

            if(bitage == null && bitage2 == null){
                //Console.WriteLine("Either Image1 or Image2, or even both, are null/empty/404(Not found)");
            }
            if(bitage.Width != bitage2.Width && bitage.Height != bitage2.Height){
                //Console.WriteLine("The images aren't the same resolution.");
            }
            }while(bitage != null && bitage2 != null && bitage.Width != bitage2.Width && bitage.Height != bitage2.Height);

        }
    }
}
