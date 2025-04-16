# Sorting Algorithms Visualizer 🚀

![logo](https://github.com/user-attachments/assets/3d585306-2f4c-4450-920b-70f95afbfbce)

Sorting Algorithms Visualizer is an interactive desktop application that brings sorting algorithms to life through real-time visualizations. Built with C# and MonoGame/XNA, this application leverages asynchronous programming for smooth, non-blocking animations and features a fully responsive, custom UI that automatically resizes according to the current resolution. 

Inspired by an engaging YouTube video on algorithm visualizations:
[Sorting Algorithms Visualization](https://www.youtube.com/watch?v=kPRA0W1kECg&t=6s)  
this project was created to provide an intuitive and educational experience for both students and enthusiasts. 

---

## Features ✨

- **Responsive User Interface**  
  - The UI scales dynamically to any resolution. Changing the resolution updates the layout, and all UI components (buttons, panels, grids) automatically resize and reposition.
  - Enjoy a modern, sleek interface with custom-designed UI components that adjust in real time.

- **Interactive Sorting Visualizations**  
  - Visualize popular sorting algorithms such as **Bubble Sort**, **Insertion Sort**, **Selection Sort**, **Merge Sort**, **Quick Sort**, **Heap Sort**, **Combo Sort**, **Cocktail Sort**, **Odd-Even Sort**, and **Randomize Shuffle**.
  - Each algorithm includes detailed animations that highlight comparisons, swaps, and key steps, all implemented using asynchronous programming techniques.

- **Asynchronous Programming**  
  - Built using C# async/await, ensuring fluid animations and a responsive user experience even during complex sorting operations.
  - The asynchronous design allows user interactions to be handled smoothly without blocking the UI.

- **Customizable Settings**  
  - Modify settings such as screen resolution, number of lines, margins, and algorithm speed.
  - Settings are persistently saved to a JSON file in the AppData folder (`%APPDATA%/Sorting_Algorithms_App/settings.json`) and loaded upon startup. If the settings file or folder does not exist, they are automatically created with default values.
  - Enjoy automatic full-screen mode when using native resolution, and easily toggle between windowed and full-screen displays.

- **Stylish UI**  
  - Includes custom logo and option panels for selecting resolution and sorting algorithms.
  - Intuitive buttons and text boxes allow quick adjustments and interact with the visualizations.
  - The UI is designed to update dynamically as you change settings, ensuring a smooth transition between different modes.

---

## Technologies Used 🛠️

- **C#** – The primary language used for development.
- **MonoGame/XNA Framework** – For graphics rendering and game loop management.
- **Asynchronous Programming (async/await)** – Ensuring non-blocking, smooth animations and user interactions.
- **JSON** – Used for saving and loading application settings.
- **Custom UI Components** – Created specifically for this project to provide a responsive and modern look.

---

## Getting Started

### Prerequisites

- [.NET Core SDK or .NET Framework](https://dotnet.microsoft.com/download)
- [MonoGame Framework](http://www.monogame.net/) (if not using a preconfigured environment)

### Installation

1. **Clone the Repository:**

```bash
  git clone https://github.com/TexablePlum/Sort_Algorithms.git
  cd Sort_Algorithms
```

2. **Build the Project:**
Open the solution file in Visual Studio (or your preferred IDE) and build the project, or use the command line:

```bash
dotnet build
```

3. **Run the Application:**

```bash
dotnet run
```


---

## Screenshots 📸

![Start Screen](https://github.com/user-attachments/assets/9a653ecc-505d-4368-855b-255e08c8af97)
*Start Screen with responsive UI and dynamic resolution changing.*

![App](https://github.com/user-attachments/assets/e3016816-5398-42d5-9110-bb4a6c2c745e)
*Live visualization of a sorting algorithm in action.*

---

## Inspiration & Acknowledgments ❤️

This project was inspired by a YouTube video on sorting algorithms visualization:
> [Sorting Algorithms Visualization](https://www.youtube.com/watch?v=kPRA0W1kECg&t=6s)

The desire to understand and interact with sorting algorithms, combined with the visual power of asynchronous programming in C#, led to the creation of this application.

---

## License 📄

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Enjoy exploring sorting algorithms! 🎉

