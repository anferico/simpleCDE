# Cloud Deployment Engine
A simple Cloud Deployment Engine (CDE) written in C#. In this project, our goal is to build a system that takes a YAML specification for a cloud application and uses it to deploy such application on a Cloud Provider. 

A YAML specification file for a cloud application looks like the one below:
```yaml
services:
    moodle:
        units: 1
    database:
        units: 1
agents:
    moodle:
        class: Moodle
        events:
            init:
                handler: install
            connect:
                after: [init, db_available]
                handler: connect
            start:
                after: [connect]
                handler: start
        requires: [db]
    database:
        class: Mysql
        events:
            init:
                handler: install
            start:
                after: [init]
                handler: start
        provides: [db]
relations:
    - [moodle: db_available, database: init]
```
The `services` section of the file lists the different services that make up the cloud application. For each of them, we have a name and a number of units (or instances) to be started. In this particular case, we have a single instance of the services `moodle` and `database`.

The different services are described within the `agents` section of the file. This section lists the class that implements the service, the events that will occur during its lifetime, a (possibly empty) list of requirements (as specified in the `requires` clause) and a (possibly empty) list of "facilities" (as specified in the `provides` clause) that can fulfill the requirements of other services. Each event is associated with a `handler`, which is the method that has to be called upon the occurence of that event. In case events need to fired sequentially, an `after` clause lists the events that must be handled before a given event can occur.

Finally, the `relations` section describes the relationships among the different services. In this case, the `moodle` service depends on the `database` service. The line `[moodle: db_available, database: init]` means that the `db_available` event of the `moodle` service will be triggered after the `init` event of the `database` service has been handled correctly.

In order to make sense of the YAML specification file for a cloud application, I've implemented a recursive descent parser for such specifications. You can find the code related to that under the [Parser](Parser) directory.

The core part of this project are the `CDE` (which stands for Cloud Deployment Engine) class (located under the [CDE](CDE) directory) and its "enhanced" version `EnhancedCDE`. 
The [CDE](CDE/CDE.cs) class features a `DeployApplication` method for deploying a cloud application based on an instance of the `Specification` class (located under [Syntactic constructs](Syntactic constructs)) that's the result of parsing a YAML specification file. In addition to that, it contains methods for instantiating single services and for triggering events.
On the other hand, the [EnhancedCDE](CDE/EnhancedCDE.cs) class also provides an `UpdateSpecification` method which is used to update the current state of the application according to an updated version of the YAML specification file for that application. For example, if the original specification file required a single instance of the `moodle` service but the new one requires 2, the cloud deployment engine will instantiate a second copy of the `moodle` service. Instead, if the new specification doesn't require any instance of `moodle` at all, the currently running instance is terminated.

Under the [Provider](Provider) directory you can find the classes that implement the cloud provider and the single services. In this case, [Moodle](Moodle.cs) implements the `moodle` service, whereas [Mysql](Mysql.cs) implements the `database` service. Since this is a toy example, the methods of such classes simply sleep for a random period of time to simulate some kind of real behavior.
