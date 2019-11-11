using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FernNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     * Fern class written by Andrew Feikema - Oct. 29, 2019
     * 
     * Bugs: WPF and shape objects are suboptimal tools for the task
     * Randomness inclusions:   Angle of primary segment of tendril called by line, 
     *                          Angle of subsequent segments
     *                          Length of subsequent segments
     * Shapes:                  Rectangle(background), Line(tendril segments), Triangle(fern base-case)
     * Sliders:                 Turn bias, Segment Size, Redux Factor
     */
    class Fern
    {
        //minimum value to draw a new tendril instead of a triangle (base-case)
        private static int TENDRILMIN = 3;
        private static double REDUX;

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: length of primary tendril segment in pixels
         * Redux: how much smaller each line segment is compared to previous
         * Turnbias: tendency of a line to turn right or left from previous line
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {
            REDUX = redux;              //sets static variable for reduction factor
            canvas.Children.Clear();    // delete old canvas contents
            // draw a new fern at the bottom center of the canvas with given parameters
            ferndraw((int)(canvas.Width / 2), (int)(canvas.Height * .9), size, turnbias, canvas);
        }

        /*
         * ferndraw draws background and an initial tendril the given location which then draws a bunch of tendrils out in 
         * regularly-spaced directions
         */
        private void ferndraw(int x, int y, double size, double turnbias, Canvas canvas)
        {
            background(canvas);
            tendril(x, y, size, turnbias, Math.PI, canvas);
        }

        //background draws the sky background and solid green foreground
        private void background(Canvas canvas)
        {
            //new brush set to background for ground up to horizon with dark green color
            SolidColorBrush myBrush = new SolidColorBrush(color: Color.FromArgb(255, 10, 100, 35));
            SolidColorBrush SkyBrush = new SolidColorBrush(color: Color.FromArgb(255, 185, 210, 255)); //color brush to draw light blue sky
            canvas.Background = myBrush; //sets background to dark green color

            Rectangle myRect = new Rectangle(); //new rectangle for sky
            myRect.MinHeight = canvas.Height * .85; //covers top 85/100ths of canvas
            myRect.Width = canvas.Width;
            myRect.Fill = SkyBrush; //fills rectangle with sky color
            canvas.Children.Add(myRect); //add rectangle to canvas
        }

        /*
         * tendril draws a tendril (a sequence of line segments) in the given direction
         * for the given length of the first segment, first points of first segment,
         * turn bias, direction, and canvas
         */
        private void tendril(int x1, int y1, double size, double turnbias, double direction, Canvas canvas)
        {
            int x2 = x1, y2 = y1;
            double size1 = size;
            double lengthfactor; //random int between .8 and 1.2 to be multiplied to randomize length of segment
            double stemfactor = .5; //multiplied to size for length of first segment
            Random random = new Random();

            while (size > TENDRILMIN)
            {   //tilts direction of line segment, with randomness of +-.06 radians
                x1 = x2; y1 = y2; //x1 and y1 set to endpoints of previous line
                direction -= (turnbias / size) + ((random.NextDouble() - .5) / 8);
                if (size1 != size) { stemfactor = 1; } //if not first line in tendril, segment is normal size
                lengthfactor = (random.NextDouble() * .4) + .8; //between .8 and 1.2
                //right addend is size to 2D dimension * reduction (if stem segment) * length randomizer
                x2 = x1 + (int)(size * stemfactor * lengthfactor * Math.Sin(direction)); //x distance determined by right addend
                y2 = y1 + (int)(size * stemfactor * lengthfactor * Math.Cos(direction)); //y distance determined by right addend
                byte red = (byte)(100 + size / 2); //color determined by size of segment
                byte green = (byte)(220 - size / 3); //darker color for longer segment
                                                     /*creates segment with coordinates of endpoints, color, thickness, 
                                                     / direction with randomness for child segments, canvas to draw on
                                                    */
                line(x1, y1, x2, y2, red, green, 0, 1 + size / 40, size, turnbias,
                    //tilts direction of line segment, with randomness of +-.16 radians
                    direction + (random.NextDouble() - .5) / 3, canvas);
                size *= REDUX; //reduce size by redux factor
            }
            //end case: draws small triange representing tiny copy of entire tree
            triangle(x2, y2, size, direction, canvas);
        }

        /*
        * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
        */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, double size, double turnbias, double direction, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush(); //color brush to draw line
            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b); //set color from arguments
            myLine.X1 = x1; //x of point 1
            myLine.Y1 = y1; //y of point 1
            myLine.X2 = x2; //x of point 2
            myLine.Y2 = y2; //y of point 2
            myLine.Stroke = mySolidColorBrush; //myColorBrush draws myLine
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness; //set thickness from argument
            canvas.Children.Add(myLine); //draws myLine

            tendril(x2, y2, size / 2.5, turnbias, direction + Math.Sign(turnbias) * 1.3, canvas); //left tendril from segment
            tendril(x2, y2, size / 2.5, -1 * turnbias, direction - Math.Sign(turnbias) * 1.3, canvas); //right tendril from segment
        }

        /*
         * triangles are the 'base case' of the tendril-line loop and is called by the tendril function
         * draws a triangle where the determined size is too small to draw a new line of tendrils
         */
        private void triangle(int x, int y, double size, double theta, Canvas canvas)
        {
            int x1 = x + (int)(size * Math.Sin(theta - Math.PI / 2)); //x of point 1 (right point)
            int y1 = y + (int)(size * Math.Cos(theta - Math.PI / 2)); //y of point 1 (right point)
            Point point1 = new Point(x1, y1);
            int x2 = x + (int)(size * Math.Sin(theta + Math.PI / 2)); //x of point 2 (left point)
            int y2 = y + (int)(size * Math.Cos(theta + Math.PI / 2)); //y of point 2 (left point)
            Point point2 = new Point(x2, y2);
            int x3 = x + (int)(size * 2 * Math.Sin(theta));//x of point 3 (top point)
            int y3 = y + (int)(size * 2 * Math.Cos(theta));//y of point 3 (top point)
            Point point3 = new Point(x3, y3);

            PointCollection points = new PointCollection() { //point collection to add to triangle
                point1,     //sets point collection to three points
                point2,
                point3 };
            Polygon triange = new Polygon();    //triangle is a polygon
            triange.Points = points;            //establish triangle's points as established points
            //paints triangle, set color to greenish
            SolidColorBrush myBrush = new SolidColorBrush(color: Color.FromArgb(255, 100, 220, 0));
            triange.Fill = myBrush;         //brush fills triangle color
            canvas.Children.Add(triange);   //add triangle to canvas
        }
    }
}
