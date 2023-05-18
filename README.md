# ![](https://i.ibb.co/23B6QsC/icon.png) Analytic Hierarchy Process (AHP)

This project is an implementation of the Analytic Hierarchy Process (AHP) algorithm in C# with a user-friendly interface created using Windows Presentation Foundation (WPF). It utilizes open-source plotting library [ScottPlot](https://scottplot.net/) for data visualization. The program includes three types of sensitivity analysis: weighted criteria attribute chart, small changes simulation, and one way sensitivity analysis.

## Features

- AHP algorithm implementation
- User-friendly interface
- Three types of sensitivity analysis:
  - Weighted criteria attribute chart[^1]
  - Small changes simulation[^2]
  - One way sensitivity analysis[^3]
- Project saving and loading
- Exporting project to csv

[^1]: Calculates the contribution of each criterion and subcriterion to the overall priority value of alternatives and visualizes the contributions in a bar chart.
[^2]: Calculates the program results with random adjustments to comparisons, records the alternative with the highest priority for each adjustment set, and presents the frequency of each alternative's "wins" in a bar chart.
[^3]: Allows users to select a comparison in the AHP model and visualize how changing its value affects alternative priorities in a graph.

## Screenshots

| ![](https://i.ibb.co/hDGsN5M/1.png) | ![](https://i.ibb.co/728pJYJ/2.png) |
|:-----------------------------------:|:-----------------------------------:|
| *Main menu*                         | *Project page*                      |
| ![](https://i.ibb.co/Jr3dHfd/3.png) | ![](https://i.ibb.co/3Mjwb03/4.png) |
| *Comparison matrix view*            | *Weighted criteria attribute chart* |
| ![](https://i.ibb.co/k5LY5Dp/5.png) | ![](https://i.ibb.co/cCQKDnR/6.png) |
| *Small changes simulation*          | *One way sensitivity analysis*      |

## Installation

To use this program, download the installer from the [Releases](https://github.com/Ivruix/AnalyticHierarchyProcess/releases) page. After downloading the installer, run it and follow the on-screen instructions to install the program on your computer.

## Usage

Once the program is installed, you can launch and use it to perform AHP analysis and sensitivity analysis.

1. Launch the program.
2. Create new project (subsequent launches of the program will also allow to open previously saved projects).
3. Enter the goal, creteria, subcriteria (if needed), and alternatives (options).
4. Perform all necessary pairwise comparisons using a handy slider scale.
5. View the results of the AHP algorithm on the project page.
6. Use the sensitivity analysis options to explore different scenarios and analyze the impact of changes on the decision-making process (prioritization of alternatives).

You can also find a sample project in the [examples](./examples) folder.

## Contributing

If you find a bug or have a feature request, please open an issue on the project's GitHub repository. Pull requests are also welcome!

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE.txt) file for details.
