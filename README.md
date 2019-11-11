# fractal_fern
Project Created Oct. 2019 for CS 212: 
In the Visual Studio IDE, these files are implemented in a WPF app using .NET Framework which creates a fractal fern from user-selected data factors
MainWindow declares the UI component
Fern.cs creates the fractal fern drawing on MainWindow's canvas

     * Bugs: WPF and shape objects are suboptimal tools for the task
     * 3 inclusions of randomness:    Angle of primary segment of tendril called by line, 
     *                                Angle of subsequent segments
     *                                Length of subsequent segments
     *
     * UI Sliders:                    Turn bias, Segment Size, Redux Factor
