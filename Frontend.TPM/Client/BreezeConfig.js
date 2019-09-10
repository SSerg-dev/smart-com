(function () {
    // ASP.NET Web API OData PRIOR to v.4
    var adapter = breeze.config.initializeAdapterInstance('dataService', 'webApiOData');
    adapter.getRoutePrefix = getRoutePrefix531; // plug-in alternative for THIS adapter instance.
    adapter.executeQuery = executeQuery;

    breeze.DataType.DateTimeOffset.fmtOData = function (val) {
        if (val == null) {
            return null;
        }
        try {
            return "datetimeoffset'" + toISOStringWithoutOffset(val) + "'";
        } catch (e) {
            throw new Error("'" + val + "' is not a valid dateTime");
        }
    }

    function toISOStringWithoutOffset(val) {
        return val.getUTCFullYear() + "-"
            + pad(val.getUTCMonth() + 1) + "-"
            + pad(val.getUTCDate()) + "T"
            + pad(val.getUTCHours()) + ":"
            + pad(val.getUTCMinutes()) + ":"
            + pad(val.getUTCSeconds()) + "."
            + pad(val.getUTCMilliseconds()) + 'Z';
    }

    function toISOStringWithOffset(val) {
        return val.getUTCFullYear() + "-"
            + pad(val.getUTCMonth() + 1) + "-"
            + pad(val.getUTCDate()) + "T"
            + pad(val.getUTCHours()) + ":"
            + pad(val.getUTCMinutes()) + ":"
            + pad(val.getUTCSeconds()) + "."
            + pad(val.getUTCMilliseconds())
            + (val.offset ? serializeOffset(val.offset) : 'Z');
    }

    function serializeOffset(offset) {
        var minutes = offset % 60;
        var hours = Math.abs((offset - minutes) / 60);
        return ((offset >= 0) ? '+' : '-') + pad(hours) + ':' + pad(minutes);
    }

    function pad(number) {
        return ((number < 10) ? '0' + number : number);
    }

    function getRoutePrefix531(dataService) {
        // Copied from breeze.debug and modified for Web API OData v.5.3.1.
        if (typeof document === 'object') { // browser
            var parser = document.createElement('a');
            parser.href = dataService.serviceName;
        } else { // node
            parser = url.parse(dataService.serviceName);
        }
        // THE CHANGE FOR 5.3.1: Add '/' prefix to pathname
        var prefix = parser.pathname;
        if (prefix[0] !== '/') {
            prefix = '/' + prefix;
        } // add leading '/'  (only in IE)
        if (prefix.substr(-1) !== '/') {
            prefix += '/';
        } // ensure trailing '/'
        return prefix;
    };


    var toODataFragmentVisitor = (function () {
        var visitor = {

            passthruPredicate: function () {
                return this.value;
            },

            unaryPredicate: function (context) {
                var predVal = this.pred.visit(context);
                return odataOpFrom(this) + " " + "(" + predVal + ")";
            },

            binaryPredicate: function (context) {
                var expr1Val = this.expr1.visit(context);
                var expr2Val = this.expr2.visit(context);
                var prefix = context.prefix;
                if (prefix) {
                    expr1Val = prefix + "/" + expr1Val;
                }

                var odataOp = odataOpFrom(this);

                if (this.op.key === 'in') {
                    var result = expr2Val.map(function (v) {
                        return "(" + expr1Val + " eq " + v + ")";
                    }).join(" or ");
                    return result;
                } else if (this.op.key === 'notcontains') {
                    return "not substringof(" + expr2Val + "," + expr1Val + ") eq true";
                } else if (this.op.isFunction) {
                    if (odataOp === "substringof") {
                        return odataOp + "(" + expr2Val + "," + expr1Val + ") eq true";
                    } else {
                        return odataOp + "(" + expr1Val + "," + expr2Val + ") eq true";
                    }
                } else {
                    return expr1Val + " " + odataOp + " " + expr2Val;
                }
            },

            andOrPredicate: function (context) {
                var result = this.preds.map(function (pred) {
                    var predVal = pred.visit(context);
                    return "(" + predVal + ")";
                }).join(" " + odataOpFrom(this) + " ");
                return result;
            },

            anyAllPredicate: function (context) {
                var exprVal = this.expr.visit(context);
                var prefix = context.prefix;
                if (prefix) {
                    exprVal = prefix + "/" + exprVal;
                    prefix = "x" + (parseInt(prefix.substring(1)) + 1);
                } else {
                    prefix = "x1";
                }
                // need to create a new context because of 'prefix'
                var newContext = breeze.core.extend({}, context);
                newContext.entityType = this.expr.dataType;
                newContext.prefix = prefix;
                var newPredVal = this.pred.visit(newContext);
                return exprVal + "/" + odataOpFrom(this) + "(" + prefix + ": " + newPredVal + ")";
            },

            litExpr: function () {
                if (Array.isArray(this.value)) {
                    return this.value.map(function (v) { return this.dataType.fmtOData(v) }, this);
                } else {
                    return this.dataType.fmtOData(this.value);
                }
            },

            propExpr: function (context) {
                var entityType = context.entityType;
                // '/' is the OData path delimiter
                return entityType ? entityType.clientPropertyPathToServer(this.propertyPath, "/") : this.propertyPath;
            },

            fnExpr: function (context) {
                var exprVals = this.exprs.map(function (expr) {
                    return expr.visit(context);
                });
                return this.fnName + "(" + exprVals.join(",") + ")";
            }
        };

        var _operatorMap = {
            'contains': 'substringof'
        };

        function odataOpFrom(node) {
            var op = node.op.key;
            var odataOp = _operatorMap[op];
            return odataOp || op;
        }

        return visitor;
    }());

    function buildUrl(entityQuery, metadataStore) {
        // force entityType validation;
        var entityType = entityQuery._getFromEntityType(metadataStore, false);
        if (!entityType) {
            // anonymous type but still has naming convention info avail
            entityType = new EntityType(metadataStore);
        }

        var queryOptions = {};
        queryOptions["$filter"] = toWhereODataFragment(entityQuery.wherePredicate);
        queryOptions["$orderby"] = toOrderByODataFragment(entityQuery.orderByClause);

        if (entityQuery.skipCount) {
            queryOptions["$skip"] = entityQuery.skipCount;
        }

        if (entityQuery.takeCount != null) {
            queryOptions["$top"] = entityQuery.takeCount;
        }

        queryOptions["$expand"] = toExpandODataFragment(entityQuery.expandClause);
        queryOptions["$select"] = toSelectODataFragment(entityQuery.selectClause);

        if (entityQuery.inlineCountEnabled) {
            queryOptions["$inlinecount"] = "allpages";
        }

        var qoText = toQueryOptionsString(queryOptions);
        var actionName = entityQuery.parameters.$actionName;
        var actionNameText = actionName ? '/' + actionName : '';
        return entityQuery.resourceName + actionNameText + qoText;

        // private methods to this func.

        function toWhereODataFragment(wherePredicate) {
            if (!wherePredicate) return undefined;
            // validation occurs inside of the toODataFragment call here.
            return wherePredicate.visit({ entityType: entityType }, toODataFragmentVisitor);
        }

        function toOrderByODataFragment(orderByClause) {
            if (!orderByClause) return undefined;
            orderByClause.validate(entityType);
            var strings = orderByClause.items.map(function (item) {
                return entityType.clientPropertyPathToServer(item.propertyPath, "/") + (item.isDesc ? " desc" : "");
            });
            // should return something like CompanyName,Address/City desc
            return strings.join(',');
        }

        function toSelectODataFragment(selectClause) {
            if (!selectClause) return undefined;
            selectClause.validate(entityType);
            var frag = selectClause.propertyPaths.map(function (pp) {
                return entityType.clientPropertyPathToServer(pp, "/");
            }).join(",");
            return frag;
        }

        function toExpandODataFragment(expandClause) {
            if (!expandClause) return undefined;
            // no validate on expand clauses currently.
            // expandClause.validate(entityType);
            var frag = expandClause.propertyPaths.map(function (pp) {
                return entityType.clientPropertyPathToServer(pp, "/");
            }).join(",");
            return frag;
        }

        function toQueryOptionsString(queryOptions) {
            var qoStrings = [];
            for (var qoName in queryOptions) {
                var qoValue = queryOptions[qoName];
                if (qoValue !== undefined) {
                    if (qoValue instanceof Array) {
                        qoValue.forEach(function (qov) {
                            qoStrings.push(qoName + "=" + encodeURIComponent(qov));
                        });
                    } else {
                        qoStrings.push(qoName + "=" + encodeURIComponent(qoValue));
                    }
                }
            }

            if (qoStrings.length > 0) {
                return "?" + qoStrings.join("&");
            } else {
                return "";
            }
        }
    };

    function executeQuery(mappingContext) {
        var deferred = breeze.Q.defer(),
            parameters = mappingContext.query.parameters,
            isPostMethod, request;

        mappingContext.dataService.uriBuilder.buildUri = buildUrl;

		var url = mappingContext.getUrl();

		// Проверка на лимит длинны URL
		if (url.length > 1800) {
			Ext.Msg.show({
				title: 'Выбрано слишком много записей',
				msg: 'Выбрано слишком много записей, привышен лимит длинны URL, уменьшите количество выбранных записей',
				buttons: Ext.MessageBox.OK,
				icon: Ext.Msg.INFO,
				fn: function () { },
				cls: 'over_all',
				closable: true
			});

			var filterString = url.substring(url.indexOf('$filter=')).replace('$filter=', '').replace('&', '');
			filterString = filterString.substring(0, filterString.indexOf('$'));

			url = url.replace('$filter=' + filterString + '&', '');
		}

        // Add query params if .withParameters was used
        if (parameters) {
            isPostMethod = parameters.$method && parameters.$method.toLocaleLowerCase() === 'post';

            //if (parameters.$actionName && !parameters.$entity) {
            //    url += '/' + encodeURIComponent(parameters.$actionName);
            //}

            var paramString = toQueryString(parameters);

            if (!Ext.isEmpty(paramString)) {
                var sep = url.indexOf('?') < 0 ? '?' : '&';
                url = url + sep + paramString;
            }
        }

        if (!isPostMethod) {
            OData.read({
                requestUri: url,
                headers: this.headers
            },
            function (data, response) {
                var inlineCount;
                if (data.__count) {
                    // OData can return data.__count as a string
                    inlineCount = parseInt(data.__count, 10);
                }
                return deferred.resolve({ results: data.results, inlineCount: inlineCount, httpResponse: response });
            },
            function (error) {
                return deferred.reject(createError(error, url));
            });
        } else {
            if (parameters.$entity && parameters.$entity.entityAspect) {
                //var uriString = '/' + getUriKey(parameters.$entity.entityAspect);// + '/' +parameters.$actionName;
                var uriString = getUriKey(parameters.$entity.entityAspect) + '/' + parameters.$actionName;
                url = mappingContext.dataService.qualifyUrl(uriString);
            }

            request = {
                headers: this.headers,
                requestUri: url,
                method: 'POST'
            };

            if (parameters.$data) {
                Ext.apply(request, { data: parameters.$data });
                //Ext.apply(request, { data: JSON.stringify(Ext.Array.from(parameters.$data)) })
            }

            OData.request(
                request,
                function (data, response) {
                    // Odata returns different result structure when it returns multiple entities (data.results) vs single entity (data directly).
                    // @see http://www.odata.org/documentation/odata-version-2-0/json-format/#RepresentingCollectionsOfEntries
                    // and http://www.odata.org/documentation/odata-version-2-0/json-format/#RepresentingEntries
                    var results;

                    if (response.statusCode == 200) {
                        results = data.results || data;
                    } else if (response.statusCode != 204) {
                        deferred.reject(createError(response, url));
                        return;
                    }

                    return deferred.resolve({ results: results, httpResponse: response });
                }, function (err) {
                    return deferred.reject(createError(err, url));
                });
        }

        return deferred.promise;
    };

    // crude serializer.  Doesn't recurse
    function toQueryString(obj) {
        var parts = [];
        for (var i in obj) {
            if (i.startsWith('$')) {
                continue;
            }

            if (obj.hasOwnProperty(i)) {
                parts.push(encodeURIComponent(i) + "=" + encodeURIComponent(obj[i]));
            }
        }
        return parts.join("&");
    };

    function getUriKey(aspect) {
        var entityType = aspect.entity.entityType;
        var resourceName = entityType.defaultResourceName;
        var kps = entityType.keyProperties;
        var uriKey = resourceName + "(";
        if (kps.length === 1) {
            uriKey = uriKey + fmtProperty(kps[0], aspect) + ")";
        } else {
            var delim = "";
            kps.forEach(function (kp) {
                uriKey = uriKey + delim + kp.nameOnServer + "=" + fmtProperty(kp, aspect);
                delim = ",";
            });
            uriKey = uriKey + ")";
        }
        return uriKey;
    };

    function fmtProperty(prop, aspect) {
        return prop.dataType.fmtOData(aspect.getPropertyValue(prop.name));
    };

    function createError(error, url) {
        // OData errors can have the message buried very deeply - and nonobviously
        // this code is tricky so be careful changing the response.body parsing.
        var result = new Error();
        var response = error && error.response;
        if (!response) {
            // in case DataJS returns "No handler for this data"
            result.message = error;
            result.statusText = error;
            return result;
        }
        result.message = response.statusText;
        result.statusText = response.statusText;
        result.status = response.statusCode;
        // non std
        if (url) result.url = url;
        result.body = response.body;
        if (response.body) {
            var nextErr;
            try {
                var body = JSON.parse(response.body);
                result.body = body;
                // OData v3 logic
                if (body['odata.error']) {
                    body = body['odata.error'];
                }
                var msg = "";
                do {
                    nextErr = body.error || body.innererror;
                    if (!nextErr) msg = msg + getMessage(body);
                    nextErr = nextErr || body.internalexception;
                    body = nextErr || body;
                } while (nextErr);
                if (msg.length > 0) {
                    result.message = msg;
                }
            } catch (e) {

            }
        }
        adapter._$impl.ctor.prototype._catchNoConnectionError(result);
        return result;
    };
})();