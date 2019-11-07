Ext.define('Ext.ux.data.proxy.Breeze', {
    extend: 'Ext.data.proxy.Proxy',
    alias: 'proxy.breeze',

    extFilterOperationMap: {
        'Equals': breeze.FilterQueryOp.Equals,
        'NotEqual': breeze.FilterQueryOp.NotEquals,
        'GraterThan': breeze.FilterQueryOp.GreaterThan,
        'GraterOrEqual': breeze.FilterQueryOp.GreaterThanOrEqual,
        'LessThan': breeze.FilterQueryOp.LessThan,
        'LessOrEqual': breeze.FilterQueryOp.LessThanOrEqual,
        'Between': null,
        'In': 'in',
        'IsNull': breeze.FilterQueryOp.Equals,
        'NotNull': breeze.FilterQueryOp.NotEquals,
        'Contains': breeze.FilterQueryOp.Contains,
        'NotContains': 'notcontains',
        'StartsWith': breeze.FilterQueryOp.StartsWith,
        'EndsWith': breeze.FilterQueryOp.EndsWith
    },

    filterOperationMap: {
        'eq': breeze.FilterQueryOp.Equals,
        'gte': breeze.FilterQueryOp.GreaterThanOrEqual,
        'lte': breeze.FilterQueryOp.LessThanOrEqual,
        'gt': breeze.FilterQueryOp.GreaterThan,
        'lt': breeze.FilterQueryOp.LessThan,
        'ne': breeze.FilterQueryOp.NotEquals,
        'like': breeze.FilterQueryOp.StartsWith,
        'conts': breeze.FilterQueryOp.Contains,
        'in': 'in',
        'notcontains': 'notcontains',
    },

    sortDirectionMap: {
        'asc': 'asc',
        'desc': 'desc'
    },

    constructor: function (config) {
        console.log('breeze proxy created', config.resourceName);
        return this.callParent(arguments);
    },

    read: function (operation, callback, scope) {
        var entityManager = this.getEntityManager(),
            query,
            request;

        query = breeze.EntityQuery.from(this.resourceName);
        query = this.applyExpand(query);
        query = this.applyFilter(operation, query);
        query = this.applyExtendedFilter(operation, query);
        query = this.applySorting(operation, query);
        query = this.applyPaging(operation, query);
        query = query.inlineCount();

        if (this.extraParams) {
			// Без actionName реквест будет отправлен GET методу в контроллер, поэтому переназначенение обязательно.
			if (this.jsonData && this.actionName) {
				query = query.withParameters({
					$extraParams: this.extraParams,
					$method: 'POST',
					$actionName: this.actionName,
					$data: {
						jsonData: this.jsonData
					}
				});
			} else {
				query = query.withParameters(this.extraParams);
			}
        }

        request = {
            callback: Ext.Function.bind(this.processResponse, this, [operation, request, callback, scope], true)
        };

        var me = this;

        entityManager.executeQuery(query).then(function (data) {
            me.traversExpand(data.results, data.query);
            Ext.callback(request.callback, this, [true, data]);
        }).fail(function (error) {
            Ext.callback(request.callback, this, [false, error]);
        });

        return request;
    },

    traversExpand: function (entities, query) {
        if (query.noTrackingEnabled) {
            return;
        }

        var expandClause = query.expandClause;

        if (expandClause == null) {
            return;
        }

        expandClause.propertyPaths.forEach(function (propertyPath) {
            var propNames = propertyPath.split('.');
            this.traversExpandPath(entities, propNames);
        }, this);
    },

    traversExpandPath: function (entities, propNames) {
        var propName = propNames[0];
        entities.forEach(function (entity) {
            var ea = entity.entityAspect;

            if (!ea) {
                return; // entity may not be a 'real' entity in the case of a projection.
            }

            var reset = this.getNavigationPropertyDestroyer(entity);
            Ext.apply(entity, reset);

            if (propNames.length === 1) {
                return;
            }

            var next = entity.getProperty(propName);

            if (!next) {
                return; // no children to process.
            }

            // strange logic because nonscalar nav values are NOT really arrays
            // otherwise we could use Array.isArray
            if (!next.arrayChanged) {
                next = [next];
            }

            this.traversExpandPath(next, propNames.slice(1));
        }, this);
    },

    create: function (operation, callback, scope) {
        var entityType = this.getBreezeEntityType(),
            request = {},
            requestCallback = Ext.Function.bind(this.processResponse, this, [operation, request, callback, scope], true);

        request.entities = operation.getRecords().map(function (record) {
            var data = this.deleteNavigationProperties(record.getData());
            return this.getEntityManager().createEntity(entityType, data);
        }, this);

        request.callback = requestCallback;

        this.getEntityManager().saveChanges().then(function (data) {
            Ext.callback(request.callback, this, [true, data.entities]);
        }).fail(function (error) {
            Ext.callback(request.callback, this, [false, error]);
        });

        return request;
    },

    update: function (operation, callback, scope) {
        var entityType = this.getBreezeEntityType(),
            request = {},
            requestCallback = Ext.Function.bind(this.processResponse, this, [operation, request, callback, scope], true);

        request.entities = operation.getRecords().map(function (record) {
            var entity = this.getEntityManager().getEntityByKey(entityType, record.getId()),
                data = this.deleteNavigationProperties(record.getData());

            var reset = this.getNavigationPropertyDestroyer(entity);
            Ext.apply(data, reset);


            return Ext.apply(entity, data);
        }, this);

        request.callback = requestCallback;

        this.getEntityManager().saveChanges().then(function (data) {
            Ext.callback(request.callback, this, [true, data.entities]);
        }).fail(function (error) {
            Ext.callback(request.callback, this, [false, error]);
        });

        return request;
    },

    destroy: function (operation, callback, scope) {
        var entityType = this.getBreezeEntityType(),
            request = {},
            requestCallback = Ext.Function.bind(this.processResponse, this, [operation, request, callback, scope], true);

        request.entities = operation.getRecords().map(function (record) {
            var entity = this.getEntityManager().getEntityByKey(entityType, record.getId());
            entity.entityAspect.setDeleted();
            return entity;
        }, this);

        request.callback = requestCallback;

        this.getEntityManager().saveChanges().then(function (data) {
            Ext.callback(request.callback, this, [true, data.entities]);
        }).fail(function (error) {
            Ext.callback(request.callback, this, [false, error]);
        });

        return request;
    },

    getBreezeEntityByRecord: function (record) {
        return this.getEntityManager().getEntityByKey(this.getBreezeEntityType(), record.getId());
    },

    // Privates.

    processResponse: function (success, response, operation, request, callback, scope) {
        var reader = this.getReader(),
            entityType = this.getBreezeEntityType(),
            entityManager = this.getEntityManager(),
            result;

        if (success === true) {
            reader.applyDefaults = false;

            if (operation.action === 'read') {
                entityManager.getChanges(entityType).forEach(function (entity) {
                    entity.entityAspect.rejectChanges();
                });
                reader.applyDefaults = true;
            }

            result = reader.read(response);

            if (result.success !== false) {
                console.log('breeze proxy request success', arguments);

                Ext.apply(operation, {
                    response: response,
                    resultSet: result
                });

                operation.commitRecords(result.records);
                operation.setCompleted();
                operation.setSuccessful();
            } else {
                console.error('breeze proxy request error', arguments);

                rejectChanges();

                operation.setException(result.message);
                this.fireEvent('exception', this, response, operation);
            }
        } else {
            if (response.body) {
                var error = response.body['odata.error'];
                if (error) {
                    var message = error.innererror ? error.innererror.message : error.message;
                    if (message == "SESSION_EXPIRED") {
                        operation.setException(response);
                        Ext.Msg.show({
                            title: l10n.ns('core').value('SessionExpiredWindowTitle'),
                            msg: l10n.ns('core').value('SessionExpiredMessage'),
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.Msg.INFO,
                            fn: function () {
                                document.location.reload(true);
                            },
                            cls: 'over_all',
                            closable: false
                        });
                    }
                }
            }
            console.error('breeze proxy request failure', arguments);

            rejectChanges();

            operation.setException(response);
            this.fireEvent('exception', this, response, operation);
        }

        Ext.callback(callback, scope || this, [operation]);

        function rejectChanges() {
            if (request.entities) {
                request.entities.forEach(function (entity) {
                    entity.entityAspect.rejectChanges();
                }, this);
            }

            delete request.entities;
        }
    },

    getBreezeEntityType: function () {
        return this.getModel().prototype.breezeEntityType;
    },

    getEntityManager: function () {
        return Ext.ux.data.BreezeEntityManager.getEntityManager();
    },

    getNavigationPropertyDestroyer: function (entity) {
        var result = {};

        var navigationProperties = entity.entityType.navigationProperties.filter(function (property) {
            return Ext.isEmpty(property.foreignKeyNames);
        }).map(function (property) {
            return property.name;
        });

        var foreignKeyProperties = entity.entityType.dataProperties.filter(function (property) {
            return property.name.match(/^\w+Id$/i) && Ext.isEmpty(entity[property.name]);
        }).map(function (property) {
            return property.name.replace(/Id$/i, '');
        });

        var propertiesToReset = Ext.Array.intersect(foreignKeyProperties, navigationProperties);

        propertiesToReset.forEach(function (name) {
            result[name] = null;
        });

        return result;
    },

    getPropertyMap: function () {
        return this.getModel().getFields().reduce(function (prev, curr) {
            prev[curr.name] = curr.mapping !== null ? curr.mapping : curr.name;
            return prev;
        }, {});
    },

    getExpandNames: function () {
        var propertyRe = /\.\w+$/i;
        var mappingRe = /^\w+(\.\w+)+$/i;

        return Ext.Array.unique(this.getModel().getFields().filter(function (field) {
            return field.mapping !== null && field.mapping.match(mappingRe);
        }, this).map(function (field) {
            return field.mapping.replace(propertyRe, '');
        }, this));
    },

    deleteNavigationProperties: function (data) {
        var propertyMap = this.getPropertyMap();

        Ext.Object.each(propertyMap, function (key, value) {
            if (value.indexOf('.') !== -1) {
                delete data[key];
            }
        }, this);

        return data;
    },

    applyExpand: function (query) {
        var expandNames = this.getExpandNames();

        if (expandNames && expandNames.length) {
            return query.expand(expandNames);
        }

        return query;
    },

    applyFilter: function (operation, query) {
        var filters = operation.filters,
            propertyMap = this.getPropertyMap(),
            predicates;

        if (operation.id) {
            var idProperty = this.getModel().prototype.idProperty;
            query = query.where(breeze.Predicate.create(idProperty, breeze.FilterQueryOp.Equals, operation.id));
        }

        if (filters && filters.length) {
            predicates = filters.map(function (filter) {
                return breeze.Predicate.create(propertyMap[filter.property], this.filterOperationMap[filter.operator], filter.value);
            }, this);

            return query.where(breeze.Predicate.and(predicates));
        }

        return query;
    },

    applyFixedFilter: function (operation, query) {
        var filters = operation.fixedFilters,
            propertyMap = this.getPropertyMap(),
            predicates = [];
        if (!filters) return query;


        for (var key in filters) {
            var filter = filters[key];
            if (filter.property && filter.operation) {
                predicates.push(breeze.Predicate.create(propertyMap[filter.property], this.extFilterOperationMap[filter.operation], filter.value));
            }
        }
        return query.where(breeze.Predicate.and(predicates));
    },

    applyExtendedFilter: function (operation, query) {
        var predicate = this.buildExtendedFilterPredicate(operation.extendedFilters);

        return predicate
            ? query.where(predicate)
            : query;
    },

    applySorting: function (operation, query) {
        var sorters = operation.sorters,
            groupers = operation.groupers,
            propertyMap = this.getPropertyMap();

        // Remotely, groupers just mean top priority sorters
        if (groupers && groupers.length) {
            // Must concat so as not to mutate passed sorters array which could be the items property of the sorters collection
            sorters = sorters ? sorters.concat(groupers) : groupers;
        }

        if (sorters && sorters.length) {
            return query.orderBy(sorters.map(function (sorter) {
                return propertyMap[sorter.property] + ' '
                    + this.sortDirectionMap[sorter.direction.toLowerCase()];
            }, this));
        } else {
            // Если не заданы поля для сортировки, то сортировать по полю Id. 
            // Необходимо для работы постраничного разбиения в odata.
            return query.orderBy(this.getModel().prototype.idProperty);
        }
    },

    applyPaging: function (operation, query) {
        if (operation.start !== undefined && operation.limit !== undefined) {
            return query.skip(operation.start).take(operation.limit);
        }

        return query;
    },

    buildExtendedFilterPredicate: function (filters) {
        function buildPredicate(node) {
            switch (Ext.typeOf(node)) {
                case 'array':
                    return node.map(function (item) {
                        return buildPredicate.apply(this, [item]);
                    }, this);
                case 'object':
                    switch (node.operator) {
                        case 'and':
                            return breeze.Predicate.and(buildPredicate.apply(this, [node.rules]));
                        case 'or':
                            return breeze.Predicate.or(buildPredicate.apply(this, [node.rules]));
                        // для фильтра по дитэйлу
                        case 'any':
                            return breeze.Predicate.create(node.entity, breeze.FilterQueryOp.Any, breeze.Predicate.and(buildPredicate.apply(this, [node.rules])));
                        default:
                            // чтобы предикат для операции Not Contains корректно создался, необходимо в файл breeze.min.js добавить notcontains:{aliases:["not substring"], isFunction: !0} туда, где инициализируется binaryPredicate
                            return breeze.Predicate.create(node.property, this.extFilterOperationMap[node.operation], node.value);
                    }
                default:
                    return null;
            }
        }

        if (filters) {
            return breeze.Predicate.and(buildPredicate.apply(this, [Ext.Array.from(filters)]));
        }

        return null;
    }

});