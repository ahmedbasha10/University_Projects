using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageFilters
{
    public class ImageOperations
    {
        /// <summary>
        /// Open an image, convert it to gray scale and load it into 2D array of size (Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of gray values</returns>
        public static byte[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            byte[,] Buffer = new byte[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x] = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x] = (byte)((int)(p[0] + p[1] + p[2]) / 3);
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }

        public static byte[] get_neighbours(byte[,] arr, int x, int y, int win_sz, int h, int w)  // O(win_sz^2)  == O(n^2)
        {
            byte[] ret = new byte[win_sz * win_sz];                                   
            int idx = 0;                                                                
            for (int i = x - (win_sz / 2); i <= x + (win_sz / 2); ++i)
            {
                for (int j = y - (win_sz / 2); j <= y + (win_sz / 2); ++j)
                {
                    if (i < 0 || j < 0 || i >= h || j >= w)
                    {
                        ret[idx] = 0;
                        idx++;
                    }
                    else
                    {
                        ret[idx] = arr[i, j];
                        idx++;
                    }
                }
            }
            return ret;
        }

        public static byte[] countsort(byte[] arr, int sz)       // O(n+k) == O(n + 256) == O(n)    n = size of array
        {
            sz *= sz;
            byte[] output = new byte[sz];
            int[] count = new int[300];

            for (int i = 0; i < 256; ++i)
                count[i] = 0;

            for (int i = 0; i < sz; ++i)
                ++count[arr[i]];

            for (int i = 1; i <= 255; ++i)
                 count[i] += count[i - 1];
            for (int i = sz - 1; i >= 0; i--)
            {
                output[count[arr[i]] - 1] = arr[i];
                --count[arr[i]];
            }
            return output;
        }
        public static byte average(byte[] arr, int trim)            // O(n)   n = arr size
        {
            int sum = 0;
            int count = 0;
            for (int i = trim; i < arr.Length - trim; ++i)  
            {
                sum += arr[i];
                ++count;
            }

            byte avg = Convert.ToByte(sum / count);
            return avg;
        }

        public static byte Select(byte[] neighbors, int trim)          // O(n*m)     n = trim value   m = neighbors list length
        {
            Dictionary<int, int> myDic = new Dictionary<int, int>();
            for (int i = 0; i < neighbors.Length; ++i)  // O(n)
            {
                myDic[i] = neighbors[i];
            }
            
            for (int i = 0; i < trim; ++i)    // O(n)
            {
                int min = 300, idx = 0;
                foreach (KeyValuePair<int, int> val in myDic)  // O(m)
                {
                    if (min > val.Value)
                    {
                        min = val.Value;
                        idx = val.Key;
                    }
                }
                myDic.Remove(idx);      // O(1)
            }
            for (int i = 0; i < trim; ++i)       // O(n)
            {
                int max = -1, idx = 0;
                foreach (KeyValuePair<int, int> val in myDic)  // O(m)
                {
                    if (max < val.Value)
                    {
                        max = val.Value;
                        idx = val.Key;
                    }
                }
                myDic.Remove(idx);      // O(1)
            }
            int sum = 0;
            foreach (KeyValuePair<int, int> val in myDic) // O(m)
            {
                sum += val.Value;
            }
            int avg = sum / myDic.Count;
            return Convert.ToByte(avg);
        }

        public static byte[,] alpha_trim(byte[,] imageMatrix, int window_size, int trim, int method)  // O(k^2 * n * m) 
        {                                                                                             // n = height  m = width k = win_sz
            int h, w;
            h = GetHeight(imageMatrix);
            w = GetWidth(imageMatrix);
            byte[,] newImageMatrix = new byte[h, w];

            byte[] arr5 = new byte[window_size * window_size];

            if (method == 0) // choose counting sort algorithm from form       // O(k^2 * n * m)
            {
                for (int i = 0; i < h; ++i)            // O(n)
                {
                    for (int j = 0; j < w; ++j)         // O(m)
                    {
                        arr5 = get_neighbours(imageMatrix, i, j, window_size, h, w); // O(k^2)
                        arr5 = countsort(arr5, window_size);                         // O(k)   
                        byte avg = average(arr5, trim);                              // O(k)                   
                        newImageMatrix[i, j] = avg;
                    }
                }

            }
            else if (method == 1)       // O(k^2 * n * m)
            {
                for (int i = 0; i < h; ++i)         // O(n)
                { 
                    for (int j = 0; j < w; ++j)      // O(m)
                    {
                        arr5 = get_neighbours(imageMatrix, i, j, window_size, h, w);  // O(k^2)
                        byte avg = Select(arr5, trim);                                // O(x*k)  x for trim value  k for arr length
                        newImageMatrix[i, j] = avg;
                    }
                }
            }
            return newImageMatrix;
        }

        public static int partition(byte[] arr, int low, int high)    // O(n)
        {
            byte pivot = arr[high];
            byte tmp;
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    tmp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = tmp;
                }
            }
            tmp = arr[high];
            arr[high] = arr[i + 1];
            arr[i + 1] = tmp;
            return (i + 1);
        }
        public static void quickSort(ref byte[] arr, int low, int high) // O(n log n)
        {
            if (low < high)
            {
                int pi = partition(arr, low, high);
                quickSort(ref arr, low, pi - 1);
                quickSort(ref arr, pi + 1, high);
            }
        }
        public static byte[] Sort(byte [] arr, int method, int sz)    // O(n log n)
        {
            if (method == 0) // O(n)
                arr = countsort(arr, sz);  // O(n)
            else if(method == 2) // O(n log n)
            {
                 quickSort(ref arr, 0, arr.Length - 1); // O(n log n)
            }
            return arr;
        }
        public static byte[,] adaptive_median(byte[,] img, int max_window, int method)   // O(n*m*k^3)
        {
            int h, w;
            h = GetHeight(img);
            w = GetWidth(img);
            byte[,] updated_image = new byte[h, w];
            
                for (int i = 0; i < h; i++)         // O(n)
                {
                    for (int j = 0; j < w; j++)     // O(m)
                    {
                        byte[] neighbours, sorted;

                        byte new_pixel_val = 0;
                        int sz = 3, a1 = 0, a2 = 0;
                        bool step2 = true;
                        while (true)            // O(k^3)
                        {
                            neighbours = get_neighbours(img, i, j, sz, h, w);      //O(k^2)
                            sorted = Sort(neighbours, method, sz);                 // O(n log n)
                            a1 = sorted[((sz * sz) / 2)] - sorted[0];
                            a2 = sorted[(sz * sz) - 1] - sorted[((sz * sz) / 2)];
                            if (a1 > 0 && a2 > 0)
                            {
                                break;
                            }
                            else
                            {
                                sz += 2;                 // O(k)     k = max window size  termination condition of while(true)
                                if (sz > max_window)
                                {
                                    sz -= 2;
                                    new_pixel_val = sorted[((sz * sz) / 2)];
                                    step2 = false;
                                    break;
                                }
                            }
                        }
                        /// step two
                        if (step2 == true)
                        {
                            neighbours = get_neighbours(img, i, j, sz, h, w);
                            sorted = Sort(neighbours, method, sz);
                            int B1 = img[i, j] - sorted[0];
                            int B2 = sorted[sz * sz - 1] - img[i, j];
                            if (B1 > 0 && B2 > 0)
                            {
                                new_pixel_val = img[i, j];
                            }
                            else
                                new_pixel_val = sorted[((sz * sz) / 2)];
                        }
                        updated_image[i, j] = new_pixel_val;
                    }
                }

            return updated_image;
        }
    }
}
