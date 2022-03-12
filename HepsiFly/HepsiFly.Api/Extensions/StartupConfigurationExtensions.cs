using System.Reflection;
using FluentValidation;
using HepsiFly.Business.Categories.Commands.CreateCategory;
using HepsiFly.Business.Categories.Commands.DeleteCategory;
using HepsiFly.Business.Categories.Commands.UpdateCategory;
using HepsiFly.Business.Categories.Queries.GetCategoriesByFilter;
using HepsiFly.Business.Categories.Queries.GetCategoryById;
using HepsiFly.Business.Products.Commands.CreateProduct;
using HepsiFly.Business.Products.Commands.DeleteProduct;
using HepsiFly.Business.Products.Commands.UpdateProduct;
using HepsiFly.Common.Exceptions;
using HepsiFly.Domain.Contracts;
using HepsiFly.Domain.Entities;
using HepsiFly.Infrastructure.Base;
using HepsiFly.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UpdateCategoryCommandValidator = HepsiFly.Business.Categories.Commands.UpdateCategory.UpdateCategoryCommandValidator;

namespace HepsiFly.Api.Extensions;

  public static class StartupConfigurationExtensions
    {
        public static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
            services.AddSingleton<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();
            services.AddSingleton<IValidator<DeleteCategoryCommand>, DeleteCategoryCommandValidator>();
            
            services.AddSingleton<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
            services.AddSingleton<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
            services.AddSingleton<IValidator<DeleteProductCommand>, DeleteProductCommandValidator>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(x => x.Errors.Select(p => p.ErrorMessage))
                        .ToList();

                    throw HepsiFlyExceptions.GetValidationException(errors);
                };
            });

            return services;
        }

        public static IServiceCollection AddMongoConfiguration(this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = configuration.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.ConnectionStringValue).Value;
                options.Database = configuration.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.DatabaseValue).Value;
            });

            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            
            return services;
        }

        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IRequestHandler<CreateCategoryCommand, Category>, CreateCategoryCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateCategoryCommand, string>, UpdateCategoryCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteCategoryCommand, string>, DeleteCategoryCommandHandler>();
            services.AddTransient<IRequestHandler<GetCategoryByIdQuery, Category>, GetCategoryByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetCategoriesByFilterQuery, List<Category>>, GetCategoriesByFilterQueryHandler>();
            
            return services;
        }
    }