using emirhan.kurtulus.api.application.Services;
using emirhan.kurtulus.api.core.Entities;
using emirhan.kurtulus.api.core.Repositories;
using emirhan.kurtulus.api.infrastructure.Mongo;
using MongoDB.Driver;

namespace emirhan.kurtulus.api.infrastructure.Repositories;

public class UserRepository(
    IMongoDatabase mongoDataBase,
    IPasswordService passwordService) : MongoRepository<User>(mongoDataBase, passwordService), IUserRepository
{
}