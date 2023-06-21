node {
    def commit_id
    stage('Checkout') {
        checkout(
            [$class: 'GitSCM', 
            branches: [[name: 'main']], 
            userRemoteConfigs: [[url: 'https://github.com/andregoncrod/StarTrekWebAPI.git']]])    
        sh "git rev-parse --short HEAD > .git/commit-id"                        
        commit_id = readFile('.git/commit-id').trim()
    }

    stage('Docker Build / Push') {
     docker.withRegistry('https://index.docker.io/v1/', 'dockerhub') {
       def app = docker.build("andregoncrod/startrekwebapi:${commit_id}", '.').push()
     }
   }
}