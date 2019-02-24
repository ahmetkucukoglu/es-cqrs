namespace ESSample.Core
{
    using Raven.Client.Documents;
    using Raven.Client.Documents.Indexes;
    using Raven.Client.Documents.Session;
    using Raven.Client.ServerWide;
    using Raven.Client.ServerWide.Operations;
    using System;
    using System.Reflection;

    public class DocumentStoreInitializer
    {
        private readonly string _url;
        private readonly string _databaseName;

        public DocumentStoreInitializer(string url, string databaseName)
        {
            _url = url;
            _databaseName = databaseName;
        }

        public DocumentStore GetDocumentStore()
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { _url },
                Database = _databaseName
            };

            documentStore.OnBeforeQuery += DocumentStore_OnBeforeQuery;
            documentStore.Initialize();

            Console.WriteLine("Connected to document store. Url: {0}. Database: {0}.", documentStore.Urls[0], documentStore.Database);

            var record = documentStore.Maintenance.Server.Send(new GetDatabaseRecordOperation(documentStore.Database));

            if (record == null)
            {
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database)));

                Console.WriteLine("Created document store database. Database: {0}.", documentStore.Database);
            }

            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), documentStore);

            Console.WriteLine("Upserted document store database index. Database: {0}.", documentStore.Database);

            return documentStore;
        }

        private void DocumentStore_OnBeforeQuery(object sender, BeforeQueryEventArgs e)
        {
            e.QueryCustomization.WaitForNonStaleResults();
        }
    }
}
