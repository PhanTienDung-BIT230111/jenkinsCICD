pipeline {
 agent any
 
 stages {
	stage('clone'){
		steps {
			echo 'Cloning source code'
			git branch:'main', url: 'https://github.com/PhanTienDung-BIT230111/jenkinsCICD.git'
		}
	} // end clone

	stage('restore package') {
		steps
		{
			echo 'Restore package'
			bat 'dotnet restore'
		}
	}

	stage ('build') {
		steps {
			echo 'build project netcore'
			bat 'dotnet build  --configuration Release'
		}
	}
	

	stage ('tests') {
		steps{
			echo 'running test...'
			bat 'dotnet test --no-build --verbosity normal'
		}
	}

	stage ('public den t thu muc')
	{
		steps{
			echo 'Publishing...'
			bat 'dotnet publish -c Release -o ./publish'
		}
	}

	stage ('Publish') {
    steps {
        echo 'Stop IIS, then copy to deploy folder'
        bat '''
            iisreset /stop
            xcopy "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\jenkinsCICD\\publish" /E /Y /I /R "c:\\wwwroot\\myproject"
            iisreset /start
        '''
    }
}


	stage('Deploy to IIS') {
            steps {
                powershell '''
               
                # Tạo website nếu chưa có
                Import-Module WebAdministration
                if (-not (Test-Path IIS:\\Sites\\MySite)) {
                    New-Website -Name "MySite" -Port 81 -PhysicalPath "c:\\wwwroot\\myproject"
                }
                '''
            }
        } // end deploy iis   
 
  } // end stages
}//end pipeline
