Ext.define('App.util.core.Util', {
    alternateClassName: 'App.Util',

    statics: {

        downloadFile: function (config) {
            config = config || {};
            var url = config.url,
                method = config.method || 'POST', // Either GET or POST. Default is POST.
                params = config.params || {};

            // Create form panel. It contains a basic form that we need for the file download.
            var form = Ext.create('Ext.form.Panel', {
                standardSubmit: true,
                url: url,
                method: method
            });

            // Call the submit to begin the file download.
            form.submit({
                target: '_blank', // Avoids leaving the page. 
                params: params
            });

            // Clean-up the form after 100 milliseconds.
            // Once the submit is called, the browser does not care anymore with the form object.
            Ext.defer(function () {
                form.close();
            }, 100);
        },

        callWhenRendered: function (cmp, callback, scope) {
            if (!cmp.rendered) {
                cmp.on({
                    single: true,
                    scope: scope || cmp,
                    afterrender: function () {
                        Ext.callback(callback, scope || cmp);
                    }
                });
            } else {
                Ext.callback(callback, scope || cmp);
            }
        },

        getNamespace: function (classFullName) {
            return classFullName.substring(0, classFullName.lastIndexOf("."));
        },

        getClassNameWithoutNamespace: function (classFullName) {
            return classFullName.substring(classFullName.lastIndexOf(".") + 1);
        },

        getSubdirectory: function (classFullName) {
            return classFullName.split('.')[2];
        },

        buildBreezeQuery: function (query, store) {
            var proxy = store.getProxy();
            var extendedFilters = store.getExtendedFilter().getFilter();
            var operation = new Ext.data.Operation({
                action: 'read',
                filters: store.filters.items,
                fixedFilters: store.fixedFilters,
                extendedFilters: extendedFilters,
                sorters: store.sorters.items,
                groupers: store.groupers.items,
                pageMapGeneration: store.data.pageMapGeneration
            });
            query = proxy.applyExpand(query);
            query = proxy.applyFilter(operation, query);
            query = proxy.applyFixedFilter(operation, query);
            query = proxy.applyExtendedFilter(operation, query);
            query = proxy.applySorting(operation, query);
            return query;
        },

        makeRequest: function (panel, resource, action, parameters, needReload, form) {
            parameters = parameters || {};
            parameters.$actionName = action;
            parameters.$method = 'POST';
            var query = breeze.EntityQuery
                .from(resource)
                .withParameters(parameters)
                .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                .execute()
                .then(function (data) {
                    panel.setLoading(false);
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    if (result.success) {
                        var msg = result.message || 'Готово';
                        App.Notify.pushInfo(msg);
                        if (needReload) {
                            panel.down('directorygrid').getStore().load();
                        }
                        if (form) {
                            form.setLoading(false);
                            form.close();
                        }
                    } else {
                        if (form) {
                            form.setLoading(false);
                        }
                        App.Notify.pushError(result.message);
                    }
                })
                .fail(function (data) {
                    panel.setLoading(false);
                    if (form) {
                        form.setLoading(false);
                    }
                    App.Notify.pushError('Ошибка при выполнении операции');
                });
        },

        // makeRequest
        makeRequestWithCallback: function (resource, action, parameters, callback, failCallback) {
            parameters = parameters || {};
            parameters.$actionName = action;
            parameters.$method = 'POST';
            var query = breeze.EntityQuery
                .from(resource)
                .withParameters(parameters)
                .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                .execute()
                .then(function (data) {
                    callback(data);
                })
                .fail(function (data) {
                    if (failCallback) {
                        failCallback(data);
                    } else {
                        App.Notify.pushError('Ошибка при выполнении операции');
                    }
                });
        },

        buildViewClassName: function (compositePanel, model, prefix, suffix) {
            if (!compositePanel || !model) {
                return '';
            }

            var panelClassFullName = Ext.getClassName(compositePanel),
                namespace = App.Util.getNamespace(panelClassFullName),
                modelClassName = App.Util.getClassNameWithoutNamespace(model.getName());

            return Ext.String.format('{0}.{1}{2}{3}', namespace, Ext.valueFrom(prefix, ''), modelClassName, Ext.valueFrom(suffix, ''));
        }
    }

});