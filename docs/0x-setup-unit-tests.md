# Setup for Unit tests

## Create the test project
Create a new project in your solution. Choose the MSTest Test Project template. 

![image](https://user-images.githubusercontent.com/25482321/150508910-02a1e4e2-20d2-42b6-9cc3-d07e55f22aef.png)

To be able to use the classes and methods you have created, these projects must be added as dependencies. 
Inside your new project, right-click on dependencies and choose "Add project reference".  
You can include both Forte.Weather.API, Forte.Weather.Services and Forte.Weather.DataAccess. 

## Create the first test
It is good practice to structure your tests the same way as you structure your code. Therefore, create a folder named *Services*, and add a file named *LocationServiceTest.cs*

Look at the automatically created file *UnitTest1.cs"*. It shows you have to structure your test file. 
The TestClass attribute denotes a class that contains unit tests. The TestMethod attribute indicates a method is a test method.

