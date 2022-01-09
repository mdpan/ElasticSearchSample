# ElasticSearchSample
An Elastic Search sample to index and search japanese documents in shared folder, database and sharepoint site. (Not fully completed for now)



**To be improved:**

1, Deletion of files is not detected by Indexing service, should be added.

2, The SharepointIndexer used MS Graph api, and the access token got in code will be expired after some time, it needs to be refreshed.

3, It's better to pass a last checked date to Indexers, so they can just check all files that updated after the last checked date

4, Using ElasticSearch Cloud on Azure.



