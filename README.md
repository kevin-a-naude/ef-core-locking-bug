# ef-core-locking-bug
A minimal example demonstrating an EntityFramework Core locking bug

## Description of bug

When two services with independent DbContext instances update different sets of tables, neither service should prevent the other from making progress and succeeding. However, this does actually occur when the first service uses a transaction, and between its call to SaveChanges and Commit, it makes a call to the second service. 

There must also be a foreign key or navigation property on a record added by the second service that references a record updated in the first service. However, neither service needs to follow the navigation property to trigger the locking behaviour.

## Steps to reproduce

1. Service A:
   * obtains an independent DbContext
   * starts a transaction
   * updates record X in table A, with Id k
   * calls SaveChanges
   * calls Service B
2. Service B:
   * obtains an independent DbContext
   * adds record Y in table B, with foreign key Id k to table A
   * calls SaveChanges <-- and locks here
3. Service A:
   * calls Commit <-- never reached

## Other Information

This was tested with multiple database providers, with similar outcomes. However, the exact exception thrown is dependent on the provider.

Database Provider:
* SqlServer : timeout exception after 30 seconds
* Sqlite : db lock exception after similar amount of time
