# Contribute

## Installation

For a local setup you first need to install **Unity 3D**. Plase note that you must use version `2017.4.26f1 LTS`. You can install it by first installing [Unity Hub](https://unity3d.com/de/get-unity/download) and then selecting the correct version in the `Installs` section.

Furthermore, make sure that you have installed the *Windows 10 Fall Creators Update*.

If you have any issues have a look at the official **Microsoft** documentation: https://docs.microsoft.com/de-de/windows/mixed-reality/install-the-tools. Please note, that their documentation is for a newer version of the [MixedRealityToolkit](https://github.com/microsoft/MixedRealityToolkit-Unity). So be careful when doing any updates as it will probably break the setup.

## Git with Unity 3D

**Unity 3D** has to be adjusted to work well with **Git**. Here is an article with all the necessary details to get you started: https://thoughtbot.com/blog/how-to-git-with-unity


## Import A-Frame Model

If you want to import a different model, created by the [Getaviz](https://github.com/softvis-research/Getaviz) generator, put the `model.html` and the `metaData.json` into the `Assets/StreamingAssets` folder. Also make sure to include the source code for your software artifact in `Assets/StreamingAssets/src/`.
