using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Commom.GlobalMethods
{
    public static class FileOperations
    {
        public static bool SaveImageToCustomPath(string imgpath, HttpPostedFile image)
        {
            bool isSuccess = true;
            if (image != null && image.ContentLength > 0)
            {
                try
                {

                    if (System.IO.File.Exists(imgpath))
                        System.IO.File.Delete(imgpath);
                    image.SaveAs(imgpath);
                }
                catch (Exception e)
                {
                    var trace = e.StackTrace;
                    isSuccess = false;
                }

            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            System.Drawing.Bitmap bmpOut = null;
            try
            {
                //Bitmap loBMP = Bitmap.FromStream(postedFile.InputStream) as Bitmap;
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

                loBMP.Dispose();
            }
            catch
            {
                return null;
            }
            return bmpOut;
        }

        public static byte[] CreateThumbnailByteArray(byte[] imageData, int lnWidth, int lnHeight)
        {
            using (MemoryStream inStream = new MemoryStream())
            {
                inStream.Write(imageData, 0, imageData.Length);
                using (System.Drawing.Image image = Bitmap.FromStream(inStream))
                {

                    //do not make image bigger
                    if (image.Width < lnWidth || image.Height < lnHeight)
                    {
                        //if no shrinking is ocurring, return the original bytes
                        return imageData;
                    }
                    else
                    {
                        using (System.Drawing.Image thumb = image.GetThumbnailImage(lnWidth, lnHeight, null, IntPtr.Zero))
                        {
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                thumb.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
                                return outStream.ToArray();
                            }
                        }
                    }
                }
            }
        }

        public static bool SaveBitmapImageToDirectory(Bitmap image, string filePath)
        {
            bool isSuccess = true;
            if (image != null)
            {
                try
                {
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 100);          // 100% Percent Compression
                    image.Save(filePath, ImageCodecInfo.GetImageEncoders()[1], encoderParameters);
                }
                catch (Exception e)
                {
                    var trace = e.StackTrace;
                    isSuccess = false;
                }

            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public static bool SaveFileToDirectory(HttpPostedFile postfile, string dPath, string fpath)
        {
            bool isSuccess = true;
            try
            {
                if (!Directory.Exists(dPath))
                {
                    Directory.CreateDirectory(dPath);
                }
                postfile.SaveAs(fpath);
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public static bool DeleteFileFromDirectory(string fpath)
        {
            bool isSuccess = true;
            try
            {
                if (File.Exists(fpath))
                {
                    File.Delete(fpath);
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public static string CreateDirectory(string path = "")
        {
            string messg = "";
            try
            {
                if (Directory.Exists(path))
                {
                    return messg = "Directory Exists Already" + path;
                }
                DirectoryInfo di = Directory.CreateDirectory(path);
                messg = "Directory Successfully Created!!" + path;
                //di.Delete();
            }
            catch (Exception e)
            {
                var trace = e.StackTrace;
                messg = "Could not Create Directory!!" + path;
            }
            finally
            {

            }
            return messg;
        }

        public static bool DeleteDirectory(string dirpath = "")
        {
            string messg = "";
            bool isSuccess = true;
            try
            {
                if (Directory.Exists(dirpath))
                {
                    Directory.Delete(dirpath, true);
                }
                else
                {
                    messg = "Path" + dirpath + "Does not Exist";
                }
            }
            catch (Exception e)
            {
                isSuccess = false;
                var trace = e.StackTrace;
                messg = "Cannot Delete Directory" + dirpath;
            }
            return isSuccess;
        }

        public static void CopyFiles(string SourcePath, string DestinationPath, string filename)
        {
            try
            {
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }
                string sourceFilePath = Path.Combine(SourcePath, filename);
                string destinationFilePath = Path.Combine(DestinationPath, filename);
                if (System.IO.File.Exists(sourceFilePath))
                {
                    if (System.IO.File.Exists(destinationFilePath))
                    {
                        System.IO.File.Delete(destinationFilePath);
                    }
                    File.Copy(sourceFilePath, destinationFilePath);
                }
            }
            catch (Exception ex)
            {
                var trace = ex.StackTrace;
            }
        }

        public static List<string> GetFileList(string path)
        {
            List<string> files = new List<string>();
            try
            {
                files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
            }
            catch (Exception ex)
            {
                files = null;
                throw ex;
            }
            return files;
        }
    }
}
