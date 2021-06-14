# Work in progress - DLRG Dresden - Event registration tool

A tool used by the DLRG Dresden to provide registration services for major events like "Jahreshauptversammlung". Registration itself will be generic and need to be extended for specific use cases. 

## Configuration settings

The Function Backend needs the follwing settings to be parsed as Environment Variables. 

| Config Flag                         | SampleValue                            | Description                                                                                                                                        |
|-------------------------------------|----------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------|
| EventRegistrationDeadline           | 07.06.2021                             | The Registration Deadline. If that date is passed, no further registration is possible. Needs to be in format: ```dd.MM.yyyy```                                                            |
| SendInBlueApiKey                    | RandomString                         | The API Key from the SendInBlue Account which will be used as E-Mail provider.                                                                     |
| RegistrationSucceededMailTemplateId | 1                                      | The Number of the SendInBlue E-Mail Template which will be used for E-Mail Notification after a successful registration                            |
| eMailNameFrom                       | MyOrganization - MyEvent               | The Name from which the E-Mail should be from.                                                                                                     |
| eMailAddressFrom                    | e-mail@myorg.random                    | The E-Mail Address where the Mail is from.                                                                                                         |
| EditAccountBaseUri                  | http://localhost:8443/EditRegistration | The BaseUrl to the Webfrontend where the user can edit its bookings. Will be automatically enhanced with the users informations as clickable link. |
| ServiceBusConnectionString          | ConnectionString                      | The connection string to the Azure Service Bus.                                                                                                    |
| StorageAccountConnectionString      | ConnectionString                     | The connection string to the Azure Storage Account.                                                                                                |
| AES_IV                              | AES_IV String                        | AES_IV String                                                                                                                                      |
| EncryptionKey                       | Encryption Key                       | The Encryption Key for AES.                                                                                                                        |
