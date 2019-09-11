Ext.define('App.util.core.ResourceBuilder', {

    constructor: function () {
        this.data = null;
        this.source = {};
    },

    /*
    Пример конфигурации.
    { // Корневое пространство имен
        'key1' : 'value1', // Определение поля
        'namespace1' : { // Определение пространства имен
            'key2' : 'value1',
        },
        'namespace2' : {
            'key3' : 'value1', 
        },
        'namespace3' : ['namespace1', 'namespace2'], // Подмешивание namespace1 и namespace2 в пустое пространство имен
        'namespace3' : {
            'mixin' : ['namespace1', 'namespace2'],  // Подмешивание namespace1 и namespace2 в пространство имен с дополнительными полями
            'key3' : 'value1'
        },
        'parent' : {
            'child1' : {
                ...
            },
            'child2' : {
                ...
            },
            'child3' : {
                'mixin' : ['child1', 'child2'], // Подмешивание по относительному пути. Поиск выполняется в пределах пространства имен родителя для child3 (parent).
                ...
            },
            'child4' : {
                'mixin' : [.namespace2', '.parent.child2'], // Подмешивание по абсолютному пути. Поиск выполняется от корня конфигурации.
                ...
            }
        }
    } 
    */
    defineConfig: function (moduleName, config) {
        // Закомментировано Для возможности переопределения логкализации ядра в модуле
        //if (this.source.hasOwnProperty(moduleName)) {
        //    console.warn(Ext.String.format('Configuration for the module \'{0}\' already defined. New configuration will be ignored.', moduleName));
        //    return;
        //}

        this.source[moduleName] = config;

        try {
            var info = this.parseConfig(this.source);
            this.data = this.executeMixin(info);
        } catch (e) {
            console.error(e);
        }
    },

    parseConfig: function (root) {
        var me = this,
            graphBuilder = this.createGraphBuilder(),
            namespaces = {},
            orderCounters = {};

        if (Ext.isObject(root)) {
            visitNode(root, '', null);
        }

        function visitNode(node, nodeFullName, parentNodeFullName) {
            var properties = {};

            orderCounters[nodeFullName] = 0;
            graphBuilder.node(nodeFullName);

            Ext.Object.each(node, function (key, value) {
                var keyInfo = me.parseKey(key);
                var nextNodeName = nodeFullName + '.' + key;

                if (!Ext.isEmpty(keyInfo.tag)) {
                    if (keyInfo.tag === 'ct') {
                        properties[key] = value;
                    } else {
                        console.warn(Ext.String.format('Unsupported tag \'{0}\' in namespace: \'{1}\'', keyInfo.tag, nodeFullName));
                    }
                } else if (Ext.isObject(value)) {
                    graphBuilder.node(nextNodeName).linkTo(nodeFullName, { type: 'insert', order: orderCounters[nodeFullName]++ });
                    visitNode(value, nextNodeName, nodeFullName);
                } else if (key === 'mixin') {
                    Ext.Array.from(value).forEach(function (mixin) {
                        if (Ext.isString(mixin)) {
                            graphBuilder.node(getFullName(mixin)).linkTo(nodeFullName, { type: 'mixin', order: orderCounters[nodeFullName]++ });
                        } else {
                            console.warn(Ext.String.format('Unsupported mixin type: \'{0}\' in namespace: \'{2}\'', mixin, nodeFullName));
                        }
                    });
                } else if (Ext.isArray(value)) {
                    graphBuilder.node(nextNodeName).linkTo(nodeFullName, { type: 'insert', order: orderCounters[nodeFullName]++ });
                    visitNode({ 'mixin': value }, nextNodeName, nodeFullName);
                } else if (Ext.isPrimitive(value)) {
                    properties[key] = value;
                } else {
                    console.warn(Ext.String.format('Unsupported value type: \'{0}:{1}\' in namespace: \'{2}\'', key, value, nodeFullName));
                }
            }, this);

            namespaces[nodeFullName] = properties;

            // TODO: potential problem if name isn't full qualified
            function getFullName(name) {
                if (name[0] === '.') {
                    return name;
                } else {
                    return (parentNodeFullName || '') + '.' + name;
                }
            }
        }

        return {
            operationGraph: graphBuilder.get(),
            namespaces: namespaces
        };
    },

    createGraphBuilder: function () {
        var graph = {};

        return {
            node: function (name) {
                makeNode(name);
                return {
                    linkTo: function (secondName, info) {
                        if (!graph[name]) {
                            graph[name] = {};
                        }

                        graph[name][secondName] = info;
                    }
                }
            },

            get: function () {
                return {
                    print: function () {
                        console.dir(graph);
                    },
                    getNodes: function () {
                        return Ext.Object.getKeys(graph);
                    },
                    getAdjacency: function (name) {
                        return graph[name];
                    },
                    topologicalSort: function () {
                        var color = {}, // 0 - not discovered, 1 - discovering, 2 - finished
                            sorted = [];

                        Ext.Object.each(graph, function (u) {
                            color[u] = 0;
                        });

                        Ext.Object.each(graph, function (u) {
                            if (color[u] === 0) {
                                dfsVisit(u);
                            }
                        });

                        function dfsVisit(u) {
                            color[u] = 1;
                            Ext.Object.each(graph[u], function (v) {
                                if (color[v] === 0) {
                                    dfsVisit(v);
                                } else if (color[v] === 1) {
                                    throw new Error(Ext.String.format('Cyclic namespace reference detected: \'{0}\' <--> \'{1}\'', v, u));
                                }
                            });
                            color[u] = 2;
                            sorted.push(u);
                        }

                        return sorted.reverse();
                    }
                };
            }
        }

        function makeNode(name) {
            if (!graph.hasOwnProperty(name)) {
                graph[name] = null;
            }
        }

    },

    executeMixin: function (info) {
        var me = this,
            namespaces = Ext.clone(info.namespaces),
            operationGraph = info.operationGraph,
            operationQueues = {},
            sequence = operationGraph.topologicalSort();

        sequence.forEach(function (srcNS) {
            Ext.Object.each(operationGraph.getAdjacency(srcNS), function (dstNS, info) {
                enqueue(srcNS, dstNS, info.type, info.order);
            });
        });

        sequence.forEach(function (dstNS) {
            var queue = operationQueues[dstNS];

            if (!queue) {
                return;
            }

            Ext.Object.each(queue, function (order, task) {
                var src = namespaces[task.src],
                    dst = namespaces[task.dst];

                if (src) {
                    switch (task.operation) {
                        case 'insert':
                            dst[task.src.substring(task.src.lastIndexOf(".") + 1)] = src;
                            break;
                        case 'mixin':
                            Ext.applyIf(dst, src);
                            break;
                        default:
                            console.error(Ext.String.format('Namespace: \'{0}\' not found', task.src));
                    }
                } else {
                    console.error(Ext.String.format('Namespace: \'{0}\' not found', task.src));
                }
            });
        });

        function enqueue(from, to, operation, order) {
            var queue;

            if (!operationQueues.hasOwnProperty(to)) {
                queue = operationQueues[to] = {};
            } else {
                queue = operationQueues[to];
            }

            queue[order] = {
                src: from,
                dst: to,
                operation: operation
            };
        }

        function flatten(root) {
            var result = {};

            visitNode(root, '');

            function visitNode(node, nodeFullName) {
                var properties = {};

                Ext.Object.each(node, function (key, value) {
                    var keyInfo = me.parseKey(key);

                    if (Ext.isObject(value) && Ext.isEmpty(keyInfo.tag)) {
                        visitNode(value, nodeFullName + '.' + keyInfo.key);
                    } else {
                        properties[keyInfo.key] = value;
                    }
                });

                result[nodeFullName] = properties;
            }

            return result;
        }

        return flatten(namespaces['']);
    },

    // TODO: find out how to know that all mixins resolved
    clearTempData: function () {
        this.source = {};
    },

    parseKey: function (key) {
        var tagSeparatorPos = key.indexOf(':');
        return {
            tag: tagSeparatorPos != -1 ? key.substring(0, tagSeparatorPos) : null,
            key: key.substring(tagSeparatorPos + 1)
        };
    }
});
