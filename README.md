# PreparationTask





docker build -f PreparationTaskService\Dockerfile -t sergbelom/preptask:latest .


kubectl apply -f kubernetes.yaml

minikube service preptask-service

