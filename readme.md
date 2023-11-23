# Lancement de l'application
### Prérequis 
- Docker - [Installation Docker](https://docs.docker.com/desktop/install/windows-install/)

### Procédure de lancement
- Clone le repository sur la branche master
  ```git
  git clone https://github.com/Dercraker/ASPNETCore_WebApi_Template.git
  ```
- Modifier le fichier docker-compose.yml
	- Sur la DB un utilisateur par défaut seras crée au premier lancement de l'app. Pour celas des variables d'environnent sont définie. Il vous faudra modifier les variable suivante : 
		- Ligne 9 : **Username={{Username}}**
		- Ligne 10 : **Password={{Password}}**
			- Les mots de passes sont soumis à une politique de sécurité stricte au seins de l'api. Pour simplifier le process et dans un carde de test il est possible d'utilisé un mot de passe tel que celui-ci : **NMdRx€HqyT8jX6** 
			  *Ce mot de passe est à utilisé uniquement dans le cadre d'un environnement local. Il ne doit j'aimais être utilisé sur un environnement de production*
		- Ligne 11 : **Email={{Email}}**
- Ouvrir un terminal à la racine du repository puis exécuter la commande suivante :
  ```docker
  docker compose up -d
  ```
- Une fois le lancement des container finis. Ce type de résultat devrais être afficher dans le terminal.
  ![[Pasted image 20231123164525.png]]
- L'api seras disponible sous ce lien : [http://localhost:8080/](http://localhost:8080/)
#### Politique de mot de passe
Les mots de passe pour être valider via l'api doivent respecter les caractéristique suivantes : 
- Contient des chiffres
- Contient des lettres minuscules
- Contient des lettre majuscules
- Contient des caractère non Alpha numérique
- La longeur minimale est de 8 caractère 
- Au minimum 4 caractère sont unnique

# Test Crud todo
Au seins de cette api il est possible de tester 2 Controller : 
- AuthController. Serviras principalement à authentifier un utilisateur. Disponible uniquement en RestFull 
- TodoTaskController. CRUD de todo task. Disponnible sous deux paradigme : RestFull et GraphQL
## Rest API
Pour tester les controllers Rest deux options sont disponible : 

### swagger
Un swagger est disponnible pour tester les différentes routes de l'api. Voici sont url [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)

Pense bien a vous authentifier au près du controller Auth > Login
Une fois le token récupérer il faudra le fournir dans la modal sous le bouton authorize

### PostMan
Il est possible d'importer la collection à partir de ce lien : [http://localhost:8080/swagger/v1/swagger.json](http://localhost:8080/swagger/v1/swagger.json)
Après avoir importer la collection. il est impératif de modifier la variable ***BaseUrl*** et de fournir cette valeur : http://localhost:8080
## Graph QL
Pour tester le controller via graphQL deux options sont disponible :  
### Banana Cake Pop
Une interface est disponible sous ce lien : [http://localhost:8080/graphql/](http://localhost:8080/graphql/)

Sur cette interface il seras possible d'avoir accès à la documentation GraphQL : New Document > Schema Reference OU Schema Defenition
Dans l'onglet Operations il seras possible de crée une requette comme suis 
```graphql
{

  todoTasks(skip: 3, take: 2, order: { taskName:ASC }) {

    items {

      taskName,

      description,

      idTask

    }

    pageInfo {

      hasNextPage,

    }

    totalCount

  }

}
```

Ce dernière permettra d'avoir la liste de toute les todo task. 
Les 3 première seront skip
Seul Deux Todo Task seront présente dans la réponse.
Les TodoTask seront trier par leurs nom dans un ordre ASC.
De plus au sein de chaque todo task seul les champs :
- TaskName
- Description
- IdTask
Seront afficher. 

Dans un objet Page info, un bool ***hasNextPage*** permet de savoir si il reste des entité à afficher en fonction des params skip et take. Cela est utile pour la pagination.
Enfin la propriété TotalCount permet de connaitre le nombre total d'entité dans la DB.

### PostMan
Ici l'approche est différente. 
- Crée une requette GraphQL 
- fournir l'url suivante : **http://localhost:8080/graphql**
- utiliser l'introspection GraphQL pour avoir accès aux donner de l'api
- Les Query disponnible et testable devrais s'afficher sous quelques seconde.


# Comparatif Paradigme 
[Voici mon document comparatif entre REST et SOAP](https://github.com/Dercraker/ASPNETCore_WebApi_Template/blob/develop/SoapVsRest.md)