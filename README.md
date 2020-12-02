# TestTask.Transaction
 
This project is in conjunction with project Identity-API. Project Identity-API is used as an authorizer to execution of requests from the current project.

To testing of this project:
1. Clone this repository and repository Identity-API
2. In files '[Name of Project].DB.Schema/DataContext.cs' and '[Name of Project].Services/appsettings.json/appsettings.Dev.json' in both projects correct the values of 'connectionString' variables: replace "Server = VLAD-NOUT-HP; ..." to "Server = [PC_NAME]; ..." where [PC_NAME] is your PC name.
3.1. Set the value '[Name of Project].DB.Schema' like Startup Project at Standard Panel and like Default Project at Package Manager Console (View -> Other Windows -> Package Manager Console) in both projects.
3.2. Execute 'Update-Database' into Package Manager Console in both projects.
3.3. Return the values back to
4. Go to https://localhost:44368/swagger/index.html
5. Complete the successful 'Register' request by entering the required details. Use a strong password!
6. Make a successful 'Login' request by entering the data specified in p.5
7. Copy the token from the response (from the 'Login' request)
8. Go to https://localhost:44317/swagger/index.html
9. Click the green 'Authorize' button on the right and paste the token into the appropriate field. Confirm by clicking the green 'Authorize' button and then click 'Close'
10. Now you can execute any necessary request from the provided list

For easy testing of the project, you can first populate the database with values. To do this, it is recommended to use the 'Merge' request in the current project with the 'data.csv' file passed to it. This file is located at the root of the current repository (as well as repository Identity-API).
