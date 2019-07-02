Ext.define('App.controller.tpm.product.ProductTree', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'producttree': {
                    afterrender: this.onTreeAfterrender,
                },
                'producttree #addNode': {
                    click: this.onAddNodeButtonClick
                },
                'producttree #deleteNode': {
                    click: this.onDeleteNodeButtonClick
                },
                'producttree basetreegrid': {
                    beforeload: this.onTreeBeforeLoad,
                    load: this.onTreeLoad,
                    itemclick: this.onTreeNodeClick,
                    beforeitemcollapse: this.beforeTreeNodeCollapse,
                    afterrender: this.onBaseTreeGridAfterRender,
                    resize: this.onBaseTreeGridResize,
                    disable: this.onBaseTreeGridDisable,
                    enable: this.onBaseTreeGridEnable,
                },
                'producttree #updateNode': {
                    click: this.onUpdateButtonClick
                },
                'producttree #moveNode': {
                    click: this.onMoveButtonClick
                },
                'producttreemovewindow #ok': {
                    click: this.onMoveConfirmButtomClick
                },
                'producttree #editFilter': {
                    click: this.onEditFilterButtonClick
                },
                'producttree #dateFilter': {
                    click: this.onDateFilterButtonClick
                },
                'producttree #productsSearchTrigger': {
                    specialkey: this.onProductTextSearch,
                    applySearch: this.applyFiltersForTree
                },

                // historicalTree
                'producttree #historicalTree': {
                    afterrender: this.onHistoricalFieldRendered,
                    select: this.onHistoricalDateSelected
                },

                'producttree #productList': {
                    click: this.onProductListButtonClick
                },
                'producttree #filteredProductList': {
                    click: this.onProductListFilteredButtonClick
                },

                //кнопки Save и Back в окне фильтра по продуктам
                'filterproduct #save': {
                    click: this.onSaveButtonClick
                },
                'filterproduct #back': {
                    click: this.onBackButtonClick
                },
                'filterproduct tool[type=close]': {
                    click: this.onBackButtonClick
                },

                // кнопки для работы с логоитипом
                'producttree #attachFile': {
                    click: this.onAttachFileButtonClick
                },
                'producttree #deleteAttachFile': {
                    click: this.onDeleteAttachFileButtonClick
                },
                '#productTreeUploadFileWindow #userOk': {
                    click: this.onUploadFileOkButtonClick
                },
            }
        });
    },

    onHistoricalDateSelected: function (field) {
        var value = field.getValue(),
            treegrid = field.up('basetreegrid'),
            tree = treegrid.up('producttree'),
            store = treegrid.store;
        store.getProxy().extraParams.dateFilter = value;
        store.load();
        var elementsToEnable = tree.query('[needDisable=true]');
        elementsToEnable.forEach(function (el) { el.disable(); });
    },
    onHistoricalFieldRendered: function (field) {
        field.triggerEl.elements[0].hide();
        field.triggerEl.elements.shift();// убираем триггер выбора текущего времени
    },

    onMoveConfirmButtomClick: function (button) {
        var window = button.up('producttreemovewindow'),
            node = window.nodeToMove,
            tree = window.down('basetreegrid'),
            store = tree.getStore(),
            selModel = tree.getSelectionModel();
        if (selModel.hasSelection()) {
            // проверки. TODO: отрефакторить после добавления приоритетов узлов
            var destinationNode = selModel.getSelection()[0];
            if (destinationNode.getId() == node.getId()) {
                App.Notify.pushInfo("Can't insert node into itself");
            } else if (destinationNode.getId() == node.get('parentId')) {
                App.Notify.pushInfo("Node is already in selected location");
            } else {
                var typesFilter = this.getTypesFilter(destinationNode, []);
                if (typesFilter.indexOf(node.get('Type')) != -1) {
                    App.Notify.pushInfo("Unable to move node here. Node can't be the same type as parents nodes");
                } else {
                    //Проверка по приоретету
                    var nodeType = node.get('Type');
                    var destinationNodeType = destinationNode.get('Type');
                    $.ajax({
                        dataType: 'json',
                        url: '/odata/NodeTypes',
                        data: {
                            $filter: "(Name eq '" + nodeType + "') or (Name eq '" + destinationNodeType + "')"
                        },
                        success: function (jsondata) {
                            if (jsondata.value.length == 2
                            || (destinationNodeType == 'root' && jsondata.value.length == 1)) {
                                var nodePriority = jsondata.value.find(function (el) {
                                    return el["Name"] == nodeType;
                                })["Priority"];
                                var destinationNodePriority = (destinationNodeType == 'root') ? 0 : jsondata.value.find(function (el) {
                                    return el["Name"] == destinationNodeType;
                                })["Priority"];
                                if (destinationNodeType == 'root' && nodePriority > 1) {
                                    App.Notify.pushInfo('Node with priority more than 1 can not be moved to root');
                                } else if (nodePriority > destinationNodePriority || destinationNodeType == 'root') {
                                    //Запрос
                                    var parameters = {
                                        nodeToMove: node.get('Id'), // TODO: При изменении записей не обновляется Id (т.к. idProperty = ObjectId), при перемещении изменённой записи будет перенесена старая запись
                                        destinationNode: destinationNode.get('Id')
                                    };
                                    window.setLoading(true);
                                    App.Util.makeRequestWithCallback('ProductTrees', 'Move', parameters,
                                        function (data) {
                                            var result = Ext.JSON.decode(data.httpResponse.data.value);
                                            if (result.success) {
                                                var masterTreeView = Ext.ComponentQuery.query('producttree')[0],
                                                    masterTree = masterTreeView.down('basetreegrid'),
                                                    store = masterTree.getStore(),
                                                    selModel = masterTree.getSelectionModel(),
                                                    proxy = store.getProxy();
                                                proxy.extraParams.filterParameter = null;
                                                // развыделяем запись
                                                selModel.clearSelections();
                                                // очищаем дерево, т.к. если не очистить иерархия может отрисовываться некорректно
                                                store.getRootNode().removeAll();
                                                // загружаем стор
                                                store.load();
                                                window.setLoading(false);
                                                window.close();
                                            } else {
                                                App.Notify.pushError(result.message || 'Error has occured');
                                                window.setLoading(false);
                                            }
                                        }, function (data) {
                                            App.Notify.pushError(data.message);
                                            window.setLoading(false);
                                        });
                                } else {
                                    App.Notify.pushInfo('Priority of destination node is less or equal of the moved one');
                                }

                            } else {
                                App.Notify.pushInfo('One or both Node Types where not found');
                            }

                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                        }
                    });
                }
            }
        } else {
            App.Notify.pushInfo('No records selected');
        }
    },

    onMoveButtonClick: function (button) {
        var tree = button.up('basetreegrid'),
            store = tree.getStore(),
            selModel = tree.getSelectionModel();

        if (selModel.hasSelection()) {
            var clienttreemovewindow = Ext.widget('producttreemovewindow');
            clienttreemovewindow.nodeToMove = selModel.getSelection()[0];
            clienttreemovewindow.show();
        } else {
            App.Notify.pushInfo('No records selected');
        }
    },

    onTreeNodeClick: function (cell, record, item, index, e, eOpts) {
        var productTree = cell.up('producttree');
        var form = productTree.down('editorform');
        var grid = cell.up(),
            addButton = productTree.down('#addNode'),
            updateButton = productTree.down('#updateNode'),
            deleteButton = productTree.down('#deleteNode'),
            moveButton = grid.down('#moveNode'),
            editFilterButton = grid.up('producttree').down('#editFilter');
        var buttonsExist = updateButton && deleteButton && moveButton && editFilterButton;
        var needEnable = buttonsExist && deleteButton.disabled && updateButton.disabled && moveButton.disabled && editFilterButton.disabled;
        if (record.isRoot()) {
            this.enableButtons([addButton]);
            this.disableButtons([updateButton, deleteButton, moveButton, editFilterButton]);
        }
        else if (record.get('EndDate') != null) {
            this.disableButtons([updateButton, deleteButton, editFilterButton, addButton]);
        }
        else /*if (needEnable)*/ {
            this.enableButtons([updateButton, deleteButton, moveButton, editFilterButton]);
        }
        if (form) {
            this.updateTreeDetail(form, record);
            var filter = this.getJsonFilter(record.get('Filter')),
                        filterConstructor = Ext.create('App.view.tpm.filter.FilterConstructior'),
                        filterProductContainer = form.down('container[name=filterProductContainer]');
            if (filter) {
                this.nodeCount = 0;
                var filterContent = this.parseJsonFilter(filter, true, filterConstructor);
                filterProductContainer.removeAll();
                filterProductContainer.add(filterContent);
                filterProductContainer.fireEvent('resize', filterProductContainer); // для качественной прокрутки
            } else {
                filterProductContainer.removeAll();
            }
        }
    },

    disableButtons: function (buttonsToDisable) {
        buttonsToDisable.forEach(function (button, index, array) {
            if (button && !button.disabled) {
                button.disable();
            }
        });
    },

    enableButtons: function (buttonsToEnable) {
        buttonsToEnable.forEach(function (button, index, array) {
            if (button && button.disabled) {
                button.enable();
            }
        });
    },

    updateTreeDetail: function (editorform, record) {
        editorform.loadRecord(record);
        this.setLogoImage(editorform.up('producttree').down('[name=treeLogoPanel]'), record.data.LogoFileName);
    },

    //ProductTree
    onTreeAfterrender: function (tree, e) {
        var treegrid = tree.down('basetreegrid'),
            promoeditorcustom = treegrid.up('promoeditorcustom'),
            promocontainer = treegrid.up('#promo'),
            store = treegrid.getStore(),
            me = this,
            updateButton = tree.down('#updateNode'),
            deleteButton = treegrid.down('#deleteNode'),
            moveButton = treegrid.down('#moveNode'),
            editFilterButton = treegrid.up('producttree').down('#editFilter'),
            proxy = store.getProxy();
        proxy.extraParams.filterParameter = null;
        me.disableButtons([updateButton, deleteButton, moveButton, editFilterButton]);
        var isCopyFromSchedule = (promoeditorcustom && promoeditorcustom.isFromSchedule && promoeditorcustom.isFromSchedule.isCopy);
        //так как автозагрузка стора отключена, необходимо загрузить стор, если иерархия открыта не в окне промо или в окне промо в режиме создания
        if (tree.needLoadTree && (!promocontainer || (promoeditorcustom && promoeditorcustom.isCreating) && !isCopyFromSchedule)) {
            store.load();
        }

        //store.on('load', function (store, node, records, success) {
        //    if (promocontainer) {
        //        if (promoeditorcustom.isCreating && !isCopyFromSchedule) {
        //            var nodes = node.childNodes.length !== 0 ? node.childNodes : records[0].childNodes.length !== 0 ? records[0].childNodes : null;
        //            me.setCheckTree(nodes, false);
        //        }
        //    }
        //})

        var promocontainer = treegrid.up('#promo');
        if (promocontainer || tree.hideNotHierarchyBtns) {
            var elementsToHide = tree.query('[hierarchyOnly=true]');
            elementsToHide.forEach(function (el) { el.hide(); });
        }

        var dateFilter = tree.down('#dateFilter');
        var currentDate = new Date();

        var days = currentDate.getDate().toString().length === 1 ? '0' + currentDate.getDate() : currentDate.getDate();
        var month = (currentDate.getMonth() + 1).toString().length === 1 ? '0' + (currentDate.getMonth() + 1) : currentDate.getMonth() + 1;
        var year = currentDate.getFullYear();

        if (dateFilter) {
            dateFilter.setText(days + '.' + month + '.' + year);
        }
    },

    setCheckTree: function (nodes, checked, targetNodes, checkedArray) {
        var me = this;
        if (nodes && nodes.length > 0) {
            nodes.forEach(function (node, index, array) {
                if (checkedArray) {
                    // для расширенного фильтра при выборе операции Список
                    var c = false;
                    Ext.Array.each(checkedArray.values, function (item, index, array) {
                        if (item.value === node.get('FullPathName')) {
                            c = true;
                        }
                    });
                    node.set('checked', c);
                } else {
                    node.set('checked', checked);
                }

                if (targetNodes && node.data.Target)
                    targetNodes.push(node);

                me.setCheckTree(node.childNodes, checked, targetNodes, checkedArray);
            });
        }
    },

    onUpdateButtonClick: function (button) {
        var tree = Ext.ComponentQuery.query('producttreegrid')[0],
            store = tree.getStore(),
            selModel = tree.getSelectionModel();

        if (selModel.hasSelection()) {
            //tree.editorModel.startEditRecord(selModel.getSelection()[0]);
            this.updateNode(tree, selModel.getSelection()[0], false);
            var productTreeEditorWindow = Ext.ComponentQuery.query('producttreeeditor')[0],
                srch = productTreeEditorWindow.down('searchcombobox');
            srch.hide();
        } else {
            console.log('No selection');
        }
    },

    onDeleteNodeButtonClick: function (button) {
        var treegrid = button.up('combineddirectorytreepanel').down('basetreegrid'),
            record = treegrid.getSelectionModel().getSelection()[0],
            msg = record.isLeaf() ? l10n.ns('tpm', 'DeleteText').value('deleteConfirmMessage') : l10n.ns('tpm', 'DeleteText').value('cascadeDeleteConfirmMessage');
        var me = this;
        Ext.Msg.show({
            title: l10n.ns('tpm', 'DeleteText').value('deleteWindowTitle'),
            msg: msg,
            fn: function (buttonId) {
                if (buttonId === 'yes') {
                    treegrid.setLoading(true);
                    var parameters = {
                        key: record.get("Id")
                    };

                    App.Util.makeRequestWithCallback('ProductTrees', 'Delete', parameters, function (data) {
                        var result = Ext.JSON.decode(data.httpResponse.data.value);
                        if (result.success) {
                            treegrid.store.load();
                            treegrid.setLoading(false);
                            //Установка на корневую запись
                            me.setToRecordNode(null);
                        } else {
                            treegrid.setLoading(false);
                        }
                    }, function (data) {
                        App.Notify.pushError(data.message);
                        treegrid.setLoading(false);
                    });
                }
            },
            scope: this,
            icon: Ext.Msg.QUESTION,
            buttons: Ext.Msg.YESNO,
            buttonText: {
                yes: l10n.ns('core', 'buttons').value('delete'),
                no: l10n.ns('core', 'buttons').value('cancel'),
            }
        });
    },

    setToRecordNode: function (record) {
        var productTree = Ext.ComponentQuery.query('producttree')[0];
        var treegrid = productTree.down('basetreegrid');
        var filterProductContainer = productTree.down('container[name=filterProductContainer]');
        var editorForm = productTree.down('editorform');
        var selModel = treegrid.getSelectionModel();
        var store = treegrid.getStore();

        if (!record) {
            var rootRecord = store.getById('1000000');
            //Установка на корневую запись
            if (rootRecord) {
                selModel.select(rootRecord);
                filterProductContainer.removeAll();
                editorForm.loadRecord(rootRecord);
            }
        } else {
            var filter = this.getJsonFilter(record.get('Filter'));
            var filterConstructor = Ext.create('App.view.tpm.filter.FilterConstructior');
            var filterProductContainer = editorForm.down('container[name=filterProductContainer]');
            var filterContent = this.parseJsonFilter(filter, true, filterConstructor);

            selModel.select(record);
            editorForm.loadRecord(record);
            if (filter) {
                this.nodeCount = 0;
                filterProductContainer.removeAll();
                filterProductContainer.add(filterContent);
            } else {
                filterProductContainer.removeAll();
            }
        }
    },

    onAddNodeButtonClick: function (button) {
        var tree = button.up('producttree').down('producttreegrid');
        var store = tree.getStore();
        var model = Ext.create(Ext.ModelManager.getModel(store.model));

        var selModel = tree.getSelectionModel();
        if (selModel.hasSelection()) {
            var record = selModel.getSelection()[0];
            // заполняем модель 
            model.set('parentId', record.get('ObjectId')); // id родительского элемента
            model.set('StartDate', new Date()); // дата, перепишется на сервер, но нужна т.к. иначе модель будет невалидна
            model.set('depth', record.get('depth') + 1); // глубина вложенности 
            //tree.editorModel.startCreateRecord(model);
            this.createNode(tree, model);

            var typesFilter = this.getTypesFilter(record, []);
            var productTreeEditorWindow = Ext.ComponentQuery.query('producttreeeditor')[0]
            var srch = productTreeEditorWindow.down('searchcombobox'),
                store = srch.getStore(),
                baseFilter = store.fixedFilters['typefilter'], // Сохраняем фильтр по типу
                ids = ['typefilter'],
                nodes = [{
                    property: baseFilter.property,
                    operation: baseFilter.operation,
                    value: baseFilter.value,
                }];
            // Фильтр по родительским узлам, для исключения вложенности узлов одного типа
            Ext.Array.each(typesFilter, function (item, index) {
                ids.push('treefilter' + index);
                nodes.push({
                    property: 'Name',
                    operation: 'NotEqual',
                    value: item
                });
            });
            //При создании корневого узла предлагать только с приорететом 1 и 0
            if (record.get('Type') == 'root') {
                ids.push('priorityrootfilter');
                nodes.push({
                    property: 'Priority',
                    operation: 'In',
                    value: [0, 1]
                });
                store.clearFixedFilters(true);
                store.setSeveralFixedFilters(ids, nodes, true);
            } else {
                //Или меньше, чем у места вставки
                $.ajax({
                    dataType: 'json',
                    url: '/odata/NodeTypes',
                    data: { $filter: "Name eq '" + record.get('Type') + "'" },
                    success: function (jsondata) {
                        if (jsondata.value[0]) {
                            var nt = jsondata.value[0];

                            ids.push('priorityfilter');
                            nodes.push({
                                property: 'Priority',
                                operation: 'GraterThan',
                                value: nt['Priority']
                            });

                            store.clearFixedFilters(true);
                            store.setSeveralFixedFilters(ids, nodes, true);
                        } else {
                            App.Notify.pushError('Node Type with name ' + record.get('Type') + ' was not found');
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                    }
                });
            }
        } else {
            App.Notify.pushInfo('No records selected');
        }
    },

    getTypesFilter: function (record, filter) {
        filter.push(record.data.Type);
        if (record.parentNode) {
            return this.getTypesFilter(record.parentNode, filter);
        } else {
            return filter;
        }
    },

    onTreeBeforeLoad: function (store) {
        if (store.ownerTree.rendered)
            store.ownerTree.mask('Loading...');
    },

    onTreeLoad: function (view, node, records, success) {
        if (success) {
            var treegrid = node.getOwnerTree(),
                selModel = treegrid.getSelectionModel(),
                store = treegrid.getStore(),
                currentRoot = store.getRootNode(),
                trueRoot = store.getById('1000000'),
                productTree = treegrid.up('producttree');

            // workaround - не нашёл способа загружать дерево вместе с рутом с сервера
            var rootIsDefault = currentRoot.getId() != '1000000';
            if (rootIsDefault) {
                store.setRootNode(trueRoot);
            }

            var mainView = treegrid.up('producttree');

            if (mainView) {
                var form = mainView.down('editorform');

                var sels = selModel.getSelection();
                if (sels.length == 0 && form && records[0]) {
                    if (records[0].get('ObjectId') == 1000000) {
                        selModel.select(records[0]);
                        treegrid.fireEvent('itemclick', treegrid.getView(), records[0]);
                    }
                }

                treegrid.unmask();

                if (treegrid.isDisabled()) {
                    treegrid.mask();
                    $('#' + treegrid.id + ' .x-mask').css('opacity', '0.1');
                }
                
                if (productTree.chooseMode) {
                    var nodes = node.childNodes.length !== 0 ? node.childNodes : records[0].childNodes.length !== 0 ? records[0].childNodes : null;
                    var targetNodes = [];

                    this.setCheckTree(nodes, false, targetNodes);
                    this.controllCheckProduct(productTree, targetNodes);
                } else if (treegrid.checkedNodes) {
                    // для расширенного фильтра при выборе операции Список
                    var nodes = node.childNodes.length !== 0 ? node.childNodes : records[0].childNodes.length !== 0 ? records[0].childNodes : null;
                    this.setCheckTree(nodes, false, null, treegrid.checkedArray);
                }

                store.getProxy().extraParams.promoId = null;
                store.getProxy().extraParams.productTreeObjectIds = null;
            }
        } else {
            App.Notify.pushError('Node is not found');
        }
    },

    beforeTreeNodeCollapse: function (node, index, item, eOpts) {
        var selModel = node.getOwnerTree().getSelectionModel();
        if (selModel.hasSelection()) {
            var rec = selModel.getSelection()[0];
            var isChildSelected = this.checkChildsSelection(node.childNodes, rec);
            if (isChildSelected)
                selModel.select(node);
        }
    },

    checkChildsSelection: function (nodes, rec) {
        var me = this;
        if (nodes.indexOf(rec) != -1) {
            return true;
        } else {
            // оставляем только узлы имеющие дочерние элементы (незагруженные так же не попадут в результат)
            var leafs = nodes.filter(function (node) {
                return node.childNodes.length > 0
            });
            if (leafs.length > 0) {
                var leafsHasSelection = false;
                leafs.forEach(function (value, index, array) {
                    if (!leafsHasSelection) {
                        // повторяем для каждого дочернего элемента пока не найдём совпадение или не переберём все
                        leafsHasSelection = me.checkChildsSelection(value.childNodes, rec);
                    }
                });
                return leafsHasSelection;
            } else {
                return false;
            }
        }
    },

    createNode: function (tree, model) {
        this.tree = tree;
        var editorModel = tree.editorModel;
        editorModel.editor = editorModel.createEditor({
            title: l10n.ns('core').value('createWindowTitle'),
            buttons: [{
                text: l10n.ns('core', 'createWindowButtons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'createWindowButtons').value('ok'),
                ui: 'green-button-footer-toolbar',
                itemId: 'create'
            }]
        });

        editorModel.editor.down('#create').on('click', this.onCreateButtonClick, this);
        editorModel.editor.down('#cancel').on('click', editorModel.onCancelButtonClick, editorModel);
        editorModel.editor.on('close', editorModel.onEditorClose, editorModel);

        editorModel.getForm().loadRecord(model);
        editorModel.editor.show();

        editorModel.editor.afterWindowShow(editorModel.editor, true);
        editorModel.getForm().getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, editorModel);

        this.editorModel = editorModel;
        this.onlyFilterUpdate = false;
    },

    updateNode: function (tree, record, onlyFilterUpdate) {
        this.tree = tree;
        var editorModel = tree.editorModel;
        editorModel.editor = editorModel.createEditor({ title: l10n.ns('core').value('updateWindowTitle') });

        editorModel.editor.down('#ok').on('click', this.onUpdateNodeButtonClick, this);
        editorModel.editor.down('#close').on('click', editorModel.onCancelButtonClick, editorModel);
        editorModel.editor.on('close', editorModel.onEditorClose, editorModel);

        editorModel.getForm().loadRecord(record);
        editorModel.editor.show();

        editorModel.editor.afterWindowShow(editorModel.editor, false);
        editorModel.getForm().getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, editorModel);

        this.editorModel = editorModel;
        this.onlyFilterUpdate = onlyFilterUpdate;

        if (onlyFilterUpdate) {
            editorModel.editor.down('#ok').fireEvent('click', editorModel.editor.down('#ok'));
        }
    },

    onEditFilterButtonClick: function (button) {
        var tree = button.up('producttree').down('producttreegrid'),
            store = tree.getStore(),
            selModel = tree.getSelectionModel();

        if (selModel.hasSelection()) {
            //tree.editorModel.startEditRecord(selModel.getSelection()[0]);
            this.updateNode(tree, selModel.getSelection()[0], true);
            var productTreeEditorWindow = Ext.ComponentQuery.query('producttreeeditor')[0],
                srch = productTreeEditorWindow.down('searchcombobox[name=Type]');
            srch.hide();
        } else {
            console.log('No selection');
        }
    },

    onCreateButtonClick: function (button) {
        var form = this.editorModel.getForm().getForm();
        if (!form.isValid()) {
            return;
        } else {
            var producttreeeditor = button.up('producttreeeditor'),
                nodeNameField = producttreeeditor.down('searchcombobox[name=Type]'),
                value = nodeNameField.getValue();
            if (value === 'Brand' || value === 'Technology') {
                // автоматическая установка фильтра по продуктам Brand - Brand flag, Technology - Supply segment
                var stringfilter = this.getStringFilter(producttreeeditor, form),
                    filter = this.getJsonFilter(stringfilter),
                    filterWindow = Ext.widget('filterproduct'),
                    filterConstructor = filterWindow.down('filterconstructor'),
                    applyButton = filterConstructor.down('#apply');

                filterConstructor.down('container[name=filtercontainer]').add(this.parseJsonFilter(filter, false, filterConstructor));
                filterWindow.show();
                filterWindow.down('product').collapse();
                filterWindow.expandProductPanel = false;
                if (stringfilter !== '') {
                    applyButton.fireEvent('click', applyButton);
                } else {
                    var selectoperationwindow = Ext.widget({
                        xtype: 'selectoperationwindow',
                        content: filterConstructor.down('container[name=filtercontainer]')
                    });
                    selectoperationwindow.show();
                }
            } else {
                var selectoperationwindow = Ext.widget({
                    xtype: 'selectoperationwindow',
                    content: null
                });
                selectoperationwindow.show();
            }
        }
    },

    onUpdateNodeButtonClick: function (button) {
        var form = this.editorModel.getForm().getForm();
        if (!form.isValid()) {
            return;
        } else {
            var filterWindow = Ext.widget('filterproduct'),
                record = form.getRecord(),
                filter = this.getJsonFilter(record.get('Filter')),
                filterConstructor = filterWindow.down('filterconstructor'),
                applyButton = filterConstructor.down('#apply');

            var producttreeeditor = button.up('producttreeeditor'),
                nodeNameField = producttreeeditor.down('searchcombobox[name=Type]'),
                value = nodeNameField.getValue();

            filterWindow.onlyFilterUpdate = this.onlyFilterUpdate;
            filterWindow.editorCloseButton = this.editorModel.editor.down('#close');

            if (!this.onlyFilterUpdate && (value === 'Brand' || value === 'Technology')) {
                // автоматическая установка фильтра по продуктам Brand - Brand flag, Technology - Supply segment
                var stringfilter = this.getStringFilter(producttreeeditor, form),
                    filter = this.getJsonFilter(stringfilter),
                    filterWindow = Ext.widget('filterproduct'),
                    filterConstructor = filterWindow.down('filterconstructor'),
                    applyButton = filterConstructor.down('#apply');

                filterConstructor.down('container[name=filtercontainer]').add(this.parseJsonFilter(filter, false, filterConstructor));
                filterWindow.show();
                filterWindow.down('product').collapse();
                filterWindow.expandProductPanel = false;
                if (stringfilter !== '') {
                    applyButton.fireEvent('click', applyButton);
                } else {
                    var selectoperationwindow = Ext.widget({
                        xtype: 'selectoperationwindow',
                        content: filterConstructor.down('container[name=filtercontainer]')
                    });
                    selectoperationwindow.show();
                }
            } else if (filter) {
                this.nodeCount = 0;
                filterConstructor.down('container[name=filtercontainer]').add(this.parseJsonFilter(filter, false, filterConstructor));
                filterWindow.show();
                filterWindow.down('product').collapse();
                filterWindow.expandProductPanel = false;
                applyButton.fireEvent('click', applyButton);
            } else {
                filterWindow.show();
                filterWindow.down('product').collapse();
                filterWindow.expandProductPanel = true;
                var selectoperationwindow = Ext.widget({
                    xtype: 'selectoperationwindow',
                    content: filterConstructor.down('container[name=filtercontainer]')
                });
                selectoperationwindow.show();
            }
        }
    },

    getStringFilter: function (editor, form) {
        var nodetype = editor.down('searchcombobox[name=Type]').getValue();
        if (nodetype === 'Brand') {
            var brandField = editor.down('searchcombobox[name=BrandId]'),
                value = brandField.getValue(),
                store = brandField.getStore();
            if (value && store.getById(value) && store.getById(value).data) {
                return '{"and": [{"BrandFlag": {"eq": "' + store.getById(value).data.Name + '" }}]}';
            }
        } else if (nodetype === 'Technology') {
            var technologyField = editor.down('searchcombobox[name=TechnologyId]'),
                value = technologyField.getValue(),
                store = technologyField.getStore(),
                treeStore = this.tree.getStore(),
                parent = treeStore.getNodeById(form.getRecord().data.parentId);

            var techNameNumber = store.find('TechnologyId', value);

            if (techNameNumber !== -1) {
                var record = store.getAt(techNameNumber);

                if (record && record.data) {
                    var technologyName = record.data.TechnologyName;

                    if (parent.data.Type === 'Brand') {
                        return '{"and": [{"BrandFlag": {"eq": "' + parent.data.Name + '" }},' +
                            '{ "SupplySegment": { "eq": "' + technologyName + '" }}]}';
                    } else {
                        return '{"and": [{ "SupplySegment": { "eq": "' + technologyName + '" }}]}';
                    }
                }
            } else if (parent.data.Type === 'Brand') {
                return '{"and": [{"BrandFlag": {"eq": "' + parent.data.Name + '" }}]}';
            }
        }
        return '';
    },

    onSaveButtonClick: function (button) {
        var productTree = Ext.ComponentQuery.query('producttree')[0];
        var productTreeGrid = productTree.down('basetreegrid');
        var store = productTreeGrid.getStore();
        var selModel = productTreeGrid.getSelectionModel();
        var sels = selModel.getSelection();
        var editorForm = productTree.down('editorform');

        var window = button.up('window'),
            stringFilter = window.stringFilter;
        var me = this;

        //if (stringFilter) {
        var form = this.editorModel.getForm().getForm(),
        record = form.getRecord();

        if (!form.isValid()) {
            return;
        }

        record.set('Filter', stringFilter);
        form.updateRecord();

        //TODO: временно (убрать запятую из Name)
        if (record.getData().Name.lastIndexOf(',') !== -1) {
            var name = record.getData().Name.substr(0, record.getData().Name.lastIndexOf(','));
            record.set('Name', name);
        }

        var errors = record.validate();
        if (!errors.isValid()) {
            form.markInvalid(errors);
            return;
        }

        console.log('record data: ', record.getData());
        this.editorModel.saveModel(record);
        button.up('window').close();

        //Сохранение выбранной записи и замена фильтра в панели


        if (sels.length > 0) {
            var storedLoadFunction = function () {
                if (record.data.Id != 0) {
                    me.setToRecordNode(record);
                } else {
                    me.setToRecordNode(null);
                }
                if (store.storedLoadFunction) {
                    store.removeListener('load', store.storedLoadFunction);
                }
            };
            store.storedLoadFunction = storedLoadFunction;
            store.on('load', storedLoadFunction);
        }
    },

    onBackButtonClick: function (button) {
        var window = button.up('filterproduct');
        if (window.onlyFilterUpdate) {
            window.editorCloseButton.fireEvent('click', window.editorCloseButton);
        };
        window.close();
    },

    getJsonFilter: function (stringFilter) {
        var textfiltermodel = Ext.create('App.extfilter.core.TextFilterModel');
        var customTextFilterModel = Ext.create('App.model.tpm.filter.CustomTextFilterModel');
        return customTextFilterModel.deserializeFilter(Ext.JSON.decode(stringFilter, true));
    },

    parseJsonFilter: function (node, filterReadOnly, constructor) {
        if (Ext.isArray(node)) {
            return Ext.Array.map(node, function (item) {
                return this.parseJsonFilter(item, filterReadOnly, constructor);
            }, this);
        } else if (Ext.isObject(node)) {
            if (node.operator) {
                var nodeConstructior = Ext.create('App.view.tpm.filter.FilterConstructiorNode'),
                    operationButton = nodeConstructior.down('#operationButton'),
                    me = this;
                nodeConstructior.down('#addNode').setVisible(!filterReadOnly);
                nodeConstructior.down('#addRule').setVisible(!filterReadOnly);
                nodeConstructior.down('#delete').setVisible(!filterReadOnly);
                nodeConstructior.down('#delete').setDisabled((this.nodeCount === 0));
                operationButton.setText(node.operator.toUpperCase());
                this.nodeCount++;

                //определение цвета фона узла в зависимости от операции
                if (node.operator === 'and') {
                    nodeConstructior.addCls('andnodefilterpanel');
                } else if (node.operator === 'or') {
                    nodeConstructior.addCls('ornodefilterpanel');
                }

                //определение отображения и отступов контейнеров (режим просмотра на боковой панели или режим создания/редактирования в окне)
                if (filterReadOnly) {
                    operationButton.addCls('filterDetailMode');
                    nodeConstructior.addCls('filterDetailMode');
                } else {
                    operationButton.removeCls('filterDetailMode');
                    nodeConstructior.removeCls('filterDetailMode');
                }

                // если в узле есть и узлы, и правила, их надо разбрасывать по соотвествующим контейнерам: content - для узлов, rulecontent - для правил
                if (Ext.isArray(node.rules)) {
                    node.rules.forEach(function (item) {
                        if (Ext.isObject(item)) {
                            if (item.operator) {
                                nodeConstructior.down('container[name=content]').add(me.parseJsonFilter(item, filterReadOnly, constructor));
                            } else {
                                nodeConstructior.down('container[name=rulecontent]').add(me.parseJsonFilter(item, filterReadOnly, constructor));
                            }
                        }
                    })
                } else {
                    var content = this.parseJsonFilter(node.rules, filterReadOnly, constructor);
                    nodeConstructior.down('container[name=content]').add(content);
                }

                return nodeConstructior;
            } else {
                var ruleContent = Ext.create('App.view.tpm.filter.FilterConstructiorRule'),
                    filterField = ruleContent.down('combobox[name=filterfield]'),
                    operationField = ruleContent.down('combobox[name=operationfield]'),
                    filterFieldValue;

                ruleContent.down('#delete').setVisible(!filterReadOnly);
                ruleContent.setMargin(filterReadOnly ? 0 : '0 0 0 60');
                operationField.setValue(this.getOperation(node.operation).text);

                if (constructor) {
                    constructor.store.data.items.forEach(function (value, index, array) {
                        if (value.data.FieldName === node.property) {
                            filterFieldValue = value.data.Field;
                        }
                    });

                    filterField.store = constructor.store;
                    filterField.setValue(filterFieldValue);
                    operationField.getStore().loadData(filterFieldValue.getAllowedOperations().map(function (op) {
                        return {
                            id: op,
                            text: l10n.ns('core', 'filter', 'operations').value(op)
                        };
                    }));
                    operationField.setValue(node.operation);
                } else {
                    filterField.setValue(node.property)
                }

                filterValue = filterField.getValue();
                var valueContainer = ruleContent.down('container[name=valuecontainer]'),
                    currentValueCmp = valueContainer.child(),
                    editorFactory = App.view.core.filter.ValueEditorFactory;

                filterValue.set('operation', node.operation);
                filterValue.set('value', null);
                var valueCmp = editorFactory.createEditor(filterValue, currentValueCmp),
                    //для операции In(Список)
                    values = node.value.values ? new App.extfilter.core.ValueList(Ext.Array.unique(node.value.values)) : null;

                if (valueCmp && valueCmp !== currentValueCmp) {
                    valueCmp.setMargin(1);
                    valueCmp.setHeight(22);
                    valueCmp.addCls('filterText');
                    valueCmp.setDisabled(filterReadOnly);
                }

                if (valueCmp !== currentValueCmp) {
                    Ext.suspendLayouts();
                    valueContainer.removeAll();
                    valueContainer.add(valueCmp);
                    Ext.resumeLayouts(true);
                }

                if (valueCmp) {
                    //для операции In(Список)
                    values ? valueCmp.setValue(values) : valueCmp.setValue(node.value);
                }

                filterField.setDisabled(filterReadOnly);
                operationField.setDisabled(filterReadOnly);

                if (filterReadOnly) {
                    ruleContent.addCls('filterDetailMode');
                    filterField.addCls('filterDetailMode');
                    operationField.addCls('filterDetailMode');
                } else {
                    ruleContent.removeCls('filterDetailMode');
                    filterField.removeCls('filterDetailMode');
                    operationField.removeCls('filterDetailMode');
                }

                return ruleContent;
            }
        } else {
            //TODO: ?
        }
    },

    getOperation: function (operation) {
        return {
            id: operation,
            text: l10n.ns('core', 'filter', 'operations').value(operation)
        };
    },

    onProductListButtonClick: function (button) {
        var productlist = Ext.widget('actualproductlist');
        var promogrid = button.up('promoeditorcustom');
        var grid = productlist.down('directorygrid');
        var store = grid.getStore();

        store.setFixedFilter('PromoId', {
            property: 'PromoId',
            operation: 'Equals',
            value: promogrid.promoId
        });

        productlist.show();
    },

    onProductListFilteredButtonClick: function (button) {
        var productlist = Ext.widget('productlist'),
            form = button.up('combineddirectorytreepanel').down('editorform'),
            filterProductContainer = form.down('container[name=filterProductContainer]'),
            c = this.getController('tpm.filter.Filter'),
            filter = c.parseConstructorFilter(filterProductContainer);

        if (filter) {
            var grid = productlist.down('directorygrid'),
                store = grid.getStore(),
                extendedFilter = store.extendedFilter;

            extendedFilter.filter = filter;
            extendedFilter.reloadStore();

            productlist.show();
        } else {
            App.Notify.pushInfo('Filter is empty');
        }
    },

    setScroll: function (tree) {
        // замена скролла в дереве
        var treeHtml = $('#' + tree.id);
        var treeViewHtml = $('#' + tree.getView().id);
        var table = treeViewHtml.find('table');
        table.css('width', 'initial');

        var heightScroll = table.length > 0 ? table.height() : 0;
        var widthScroll = table.length > 0 ? table.innerWidth() : 0;
        var jspV = $('#vScrollProductTree' + tree.id);
        var jspH = $('#hScrollProductTree' + tree.id);

        // если скролла есть, то обновить, иначе создать
        if (jspV.length > 0) {
            jspV.height(treeViewHtml.height());
            $('#vScrollProductTreeDiv' + tree.id).height(heightScroll);
            jspV.data('jsp').reinitialise();
            $('#vScrollProductTree' + tree.id).data('jsp').scrollToY(treeViewHtml.scrollTop());
        } else {
            treeViewHtml.css('overflow', 'hidden');
            treeHtml.append('<div id="vScrollProductTree'+ tree.id +'" class="vScrollTree scrollpanel" style="height: ' + treeViewHtml.height() + 'px;">'
                + '<div id="vScrollProductTreeDiv' + tree.id +'" style="height: ' + heightScroll + 'px;"></div></div>');

            $('#vScrollProductTree' + tree.id).jScrollPane();
            $('#vScrollProductTree' + tree.id).data('jsp').scrollToY(treeViewHtml.scrollTop());
            $('#vScrollProductTree' + tree.id).on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
                treeViewHtml.scrollTop(scrollPositionY);
                return false;
            });
        }

        if (jspH.length > 0) {
            jspH.width(treeViewHtml.width());
            $('#hScrollProductTreeDiv' + tree.id).width(widthScroll);
            jspH.data('jsp').reinitialise();
            $('#hScrollProductTree' + tree.id).data('jsp').scrollToX(treeViewHtml.scrollLeft());
        } else {
            treeHtml.append('<div id="hScrollProductTree' + tree.id +'" class="hScrollTree scrollpanel">'
                + '<div id="hScrollProductTreeDiv' + tree.id +'" style="width: ' + widthScroll + 'px;"></div></div>');

            $('#hScrollProductTree' + tree.id).jScrollPane();
            $('#hScrollProductTree' + tree.id).data('jsp').scrollToX(treeViewHtml.scrollLeft());
            $('#hScrollProductTree' + tree.id).on('jsp-scroll-x', function (event, scrollPositionX, isAtTop, isAtBottom) {
                treeViewHtml.scrollLeft(scrollPositionX);
                return false;
            });
        }
    },

    onBaseTreeGridAfterRender: function (tree) {
        var me = this;
        var treeViewHtml = $('#' + tree.getView().id);
        me.setScroll(tree);
        $('#' + tree.getView().id).on("DOMSubtreeModified", function () {
            var table = treeViewHtml.find('table');
            var heightTable = table.height();
            var widthTable = table.innerWidth();
            var currentHeightScroll = $('#vScrollProductTreeDiv' + tree.id).height();
            var currentWidthScroll = $('#hScrollProductTreeDiv' + tree.id).width();

            if ((heightTable && heightTable != 0 && currentHeightScroll && heightTable != currentHeightScroll)
                || (widthTable && widthTable != 0 && currentWidthScroll && widthTable != currentWidthScroll))
                me.setScroll(tree);

            return false;
        });

        $('#' + tree.getView().id).on('wheel', function (e) {
            var direction = e.originalEvent.deltaY > 0 ? 1 : -1;
            var scrollValue = this.scrollTop + direction * 40;

            $('#vScrollProductTree' + tree.id).data('jsp').scrollToY(scrollValue);
            return false;
        });

        $('#' + tree.getView().id).on('scroll', function (e) {
            if (this.scrollTop != $('#vScrollProductTree' + tree.id).scrollTop())
                $('#vScrollProductTree' + tree.id).data('jsp').scrollToY(this.scrollTop);
        });

        tree.mask('Loading...');
        this.checkRolesAccess(tree);
    },

    onBaseTreeGridResize: function (tree) {
        this.setScroll(tree);

        // выставляем подпись загрузки по середине
        var xMask = $('#' + tree.id + ' .x-mask');
        var xMsg = $('#' + tree.id + ' .x-mask-msg');

        xMsg.css('top', xMask.height() / 2 - xMsg.height() / 2 + 'px');
        xMsg.css('left', xMask.width() / 2 - xMsg.width() / 2 + 'px');
    },

    onDateFilterButtonClick: function (button) {
        var me = this;
        var datetimeField = Ext.widget('datetimefield');
        datetimeField.dateFormat = 'd.m.Y';
        var datetimePicker = Ext.widget('datetimepicker', {
            pickerField: datetimeField,
            title: datetimeField.selectorTitle,
            width: datetimeField.selectorWidth,
            height: datetimeField.selectorHeight,
            value: button.dateValue || new Date(),
            listeners: {
                scope: datetimeField,
                select: datetimeField.onSelect
            },
            increment: datetimeField.increment
        });

        datetimePicker.show();

        var productTree = button.up('producttree');
        var okButton = Ext.ComponentQuery.query('button[action="ok"]')[0];
        var dateFilterButton = Ext.ComponentQuery.query('#dateFilter')[0];
        var productTreeGrid = productTree.down('producttreegrid');
        var store = productTreeGrid.store;

        okButton.addListener('click', function () {
            var resultDate = datetimeField.getValue();
            var days = resultDate.getDate().toString().length === 1 ? '0' + resultDate.getDate() : resultDate.getDate();
            var month = (resultDate.getMonth() + 1).toString().length === 1 ? '0' + (resultDate.getMonth() + 1) : resultDate.getMonth() + 1;
            var year = resultDate.getFullYear();
            button.dateValue = resultDate;
            dateFilterButton.setText(days + '.' + month + '.' + year);
            store.getProxy().extraParams.dateFilter = resultDate;
            me.applyFiltersForTree(productTree);
        });
    },

    onBaseTreeGridDisable: function (grid) {
        var toolbar = grid.up('producttree').down('customtoptreetoolbar');
        toolbar.down('#productsSearchTrigger').setDisabled(true);
    },

    onBaseTreeGridEnable: function (grid) {
        var toolbar = grid.up('producttree').down('customtoptreetoolbar');
        toolbar.down('#productsSearchTrigger').setDisabled(false);
    },

    checkRolesAccess: function (grid) {
        var productTree = grid.up('producttree');
        if (productTree) {
            var resource = productTree.getBaseModel().getProxy().resourceName;
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;

            // кнопки которые проверяем на доступ
            var btns = ['#addNode', '#deleteNode', '#updateNode', '#editFilter', '#attachFileName' , '#attachFile' , '#deleteAttachFile'];

            Ext.each(btns, function (btnName) {
                var btn = productTree.down(btnName);
                var access = pointsAccess.find(function (element) {
                    return element.Resource == resource && element.Action == btn.action;
                });

                if (!access)
                    btn.setVisible(false);
            });
        }
    },

    onProductTextSearch: function (field, e) {
        if (!e || e.getKey() == e.ENTER) {
            var productTree = field.up('producttree');
            this.applyFiltersForTree(productTree);
        }
    },

    applyFiltersForTree: function (productTree) {
        var textFieldSearch = productTree.down('#productsSearchTrigger');
        var store = productTree.down('basetreegrid').store;
        var proxy = store.getProxy();

        var textSearch = textFieldSearch.getValue()
        if (textSearch && textSearch.length > 0 && textSearch.indexOf('Product search') == -1)
            proxy.extraParams.filterParameter = textSearch;

        if (productTree.choosenClientObjectId.length > 0) {
            productTree.choosenClientObjectId.forEach(function (objectId, index) {
                proxy.extraParams.productTreeObjectIds += objectId;

                if (index != productTree.choosenClientObjectId.length - 1)
                    proxy.extraParams.productTreeObjectIds += ';';
            });
        }
        else {
            proxy.extraParams.productTreeObjectIds = null;
        }

        store.getRootNode().removeAll();
        store.getRootNode().setId('root');
        store.load();
        proxy.extraParams.filterParameter = null;
    },

    // следит за галочками
    controllCheckProduct: function (productTree, targetNodes) {        
        var productTreeGrid = productTree.down('basetreegrid');

        if (targetNodes && targetNodes.length > 0) {
            var selectionModel = productTreeGrid.getSelectionModel();
            var countChecked = productTreeGrid.getChecked().length;

            targetNodes.forEach(function (node) {
                productTreeGrid.fireEvent('checkchange', node, true);
            });

            productTreeGrid.fireEvent('itemclick', productTreeGrid.down('treeview'), targetNodes[0]);

            if (productTreeGrid.isDisabled()) {
                selectionModel.deselectAll();
            }
            else if (countChecked == 0 && targetNodes[0].parentNode.isExpanded()) {
                selectionModel.select(targetNodes[0]);
                this.scrollNodeToCenterTree(productTreeGrid, targetNodes[0]);
            }
        }
    },

    scrollNodeToCenterTree: function (grid, node) {
        if (node) {
            var selectionModel = grid.getSelectionModel();
            var view = selectionModel.view;
            var store = selectionModel.getStore();
            var totalRows = store.getCount();
            var recIndex = store.indexOf(node);
            var rowsVisible = view.getHeight() / view.getNode(0).offsetHeight;
            var halfScreen = rowsVisible / 2;

            if (totalRows > rowsVisible) {
                if (recIndex + halfScreen > totalRows - 1) {
                    view.focusRow(totalRows - 1);
                }
                else {
                    view.focusRow(0);
                    view.focusRow(recIndex + Math.floor(halfScreen));
                }
            }

            view.focusRow(node);
        }
    },

    onAttachFileButtonClick: function (button) {
        var resource = 'ProductTrees';
        var action = 'UploadLogoFile';

        var uploadFileWindow = Ext.widget('uploadfilewindow', {
            itemId: 'productTreeUploadFileWindow',
            resource: resource,
            action: action,
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'buttons').value('upload'),
                ui: 'green-button-footer-toolbar',
                itemId: 'userOk'
            }]
        });

        uploadFileWindow.down('filefield').regex = /^.*\.(png|jpg)$/,
        uploadFileWindow.down('filefield').regexText = 'Only .png or .jpg file supported',
        uploadFileWindow.show();
    },

    onDeleteAttachFileButtonClick: function (button) {
        var me = this;
        var productTree = button.up('producttree');
        var logoPanel = productTree.down('[name=treeLogoPanel]');
        var currentNode = productTree.down('producttreegrid').getSelectionModel().getSelection()[0];

        var parameters = {
            id: currentNode.get('Id')
        };

        App.Util.makeRequestWithCallback('ProductTrees', 'DeleteLogo', parameters,
            function (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    currentNode.data.LogoFileName = null;
                    me.setLogoImage(logoPanel, null);
                } else {
                    App.Notify.pushError(result.message || 'Error has occured');
                }
            },
            function (data) {
                App.Notify.pushError(data.message);
            }
        );
    },

    onUploadFileOkButtonClick: function (button) {
        var me = this;
        var productTree = Ext.ComponentQuery.query('producttree')[0];
        var logoPanel = productTree.down('[name=treeLogoPanel]');
        var win = button.up('uploadfilewindow');
        var currentNode = productTree.down('producttreegrid').getSelectionModel().getSelection()[0];
        var url = Ext.String.format("/odata/{0}/{1}?productTreeId={2}", win.resource, win.action, currentNode.get('Id'));
        var needCloseParentAfterUpload = win.needCloseParentAfterUpload;
        var parentWin = win.parentGrid ? win.parentGrid.up('window') : null;
        var form = win.down('#importform');
        var paramform = form.down('importparamform');
        var isEmpty;
        if (paramform) {
            var constrains = paramform.query('field[isConstrain=true]');
            isEmpty = constrains && constrains.length > 0 && constrains.every(function (item) {
                return Ext.isEmpty(item.getValue());
            });

            if (isEmpty) {
                paramform.addCls('error-import-form');
                paramform.down('#errormsg').getEl().setVisible();
            }
        }
        if (form.isValid() && !isEmpty) {
            form.getForm().submit({
                url: url,
                waitMsg: l10n.ns('core').value('uploadingFileWaitMessageText'),
                success: function (fp, o) {
                    // Проверить ответ от сервера на наличие ошибки и отобразить ее, в случае необходимости
                    if (o.result) {
                        win.close();
                        if (parentWin && needCloseParentAfterUpload) {
                            parentWin.close();
                        }
                        var pattern = '/odata/ProductTrees/DownloadLogoFile?fileName={0}';
                        var downloadFileUrl = document.location.href + Ext.String.format(pattern, o.result.fileName);

                        var attachFileName = Ext.ComponentQuery.query('#attachFileName')[0];
                        attachFileName.setValue('<a href=' + downloadFileUrl + '>' + o.result.fileName + '</a>');
                        attachFileName.attachFileName = o.result.fileName;

                        var currentNode = Ext.ComponentQuery.query('producttreegrid')[0].getSelectionModel().getSelection()[0];
                        currentNode.data.LogoFileName = o.result.fileName;
                        me.setLogoImage(logoPanel, o.result.fileName);

                        App.Notify.pushInfo(win.successMessage || 'Файл был загружен на сервер');
                    } else {
                        App.Notify.pushError(o.result.message);
                    }
                },
                failure: function (fp, o) {
                    App.Notify.pushError(o.result.message || 'Ошибка при обработке запроса');
                }
            });
        }
    },

    setLogoImage: function (logoPanel, logoFileName) {
        var logoImage = logoPanel.down('#logoImage');
        var attachFileName = logoPanel.down('#attachFileName');
        var deleteAtachFile = logoPanel.down('#deleteAttachFile');

        if (logoFileName) {
            var pattern = '/odata/ProductTrees/DownloadLogoFile?fileName={0}';
            var downloadFileUrl = document.location.href + Ext.String.format(pattern, logoFileName);

            attachFileName.setValue('<a href=' + downloadFileUrl + '>' + logoFileName + '</a>');
            attachFileName.attachFileName = logoFileName;            
            logoImage.setSrc(downloadFileUrl);
            deleteAtachFile.setDisabled(false);
        } else {
            logoImage.setSrc('/bundles/style/images/swith-glyph-gray.png');            
            attachFileName.setValue(null);
            deleteAtachFile.setDisabled(true);
        }
    }
});