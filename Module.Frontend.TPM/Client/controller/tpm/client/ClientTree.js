Ext.define('App.controller.tpm.client.ClientTree', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'clienttree': {
                    afterrender: this.onTreeAfterrender
                },
                'clienttree #addNode': {
                    click: this.onAddNodeButtonClick
                },
                'clienttree basetreegrid': {
                    beforeload: this.onTreeBeforeLoad,
                    load: this.onTreeLoad,
                    select: this.onTreeNodeSelect,
                    beforeitemcollapse: this.beforeTreeNodeCollapse,
                    itemexpand: this.onTreeNodeExpandCollapse,
                    itemcollapse: this.onTreeNodeExpandCollapse,
                    checkchange: this.onCheckChange,
                    afterrender: this.onBaseTreeGridAfterRender,
                    resize: this.onBaseTreeGridResize,
                    disable: this.onBaseTreeGridDisable,
                    enable: this.onBaseTreeGridEnable,
                },
                'clienttreeeditor': {
                    afterrender: this.onClientTreeEditorAfterRender,
                    close: this.onClientTreeEditorClose
                },
                'clienttree #baseClientsCheckbox': {
                    change: this.onBaseClientsCheckbox
                },
                'clienttree #clientsSearchTrigger': {
                    specialkey: this.onClientTextSearch,
                    applySearch: this.applyFiltersForTree
                },
                'clienttree #deleteNode': {
                    click: this.onDeleteNodeButtonClick
                },
                'clienttree #updateNode': {
                    click: this.onUpdateButtonClick
                },
                'clienttree #moveNode': {
                    click: this.onMoveButtonClick
                },
                'clienttreemovewindow #ok': {
                    click: this.onMoveConfirmButtomClick
                },
                'clienttree #historicalTree': {
                    afterrender: this.onHistoricalFieldRendered,
                    select: this.onHistoricalDateSelected
                },
                'clienttree #dateFilter': {
                    click: this.onDateFilterButtonClick
                },
                'clienttreeeditor [name=IsBeforeStart]': {
                    change: this.onIsBeforeStartChange
                },
                'clienttreeeditor [name=IsBeforeEnd]': {
                    change: this.onIsBeforeEndChange
                },
                'clienttree #attachFile': {
                    click: this.onAttachFileButtonClick
                },
                'clienttree #deleteAttachFile': {
                    click: this.onDeleteAttachFileButtonClick
                },
                '#clientTreeUploadFileWindow #userOk': {
                    click: this.onUploadFileOkButtonClick
                },
            }
        });
    },
    onHistoricalDateSelected: function (field) {
        var value = field.getValue(),
            treegrid = field.up('basetreegrid'),
            store = treegrid.store;
        store.getProxy().extraParams.dateFilter = value;
        store.load();
        var elementsToEnable = treegrid.query('[needDisable=true]');
        elementsToEnable.forEach(function (el) { el.disable(); });
    },
    onHistoricalFieldRendered: function (field) {
        field.triggerEl.elements[0].hide();
        field.triggerEl.elements.shift();// убираем триггер выбора текущего времени
    },
    onMoveConfirmButtomClick: function (button) {
        var window = button.up('clienttreemovewindow'),
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
                                    App.Util.makeRequestWithCallback('ClientTrees', 'Move', parameters,
                                        function (data) {
                                            var result = Ext.JSON.decode(data.httpResponse.data.value);
                                            if (result.success) {
                                                var masterTreeView = Ext.ComponentQuery.query('clienttree')[0],
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
            var clienttreemovewindow = Ext.widget('clienttreemovewindow');
            clienttreemovewindow.nodeToMove = selModel.getSelection()[0];
            clienttreemovewindow.show();
        } else {
            App.Notify.pushInfo('No records selected');
        }
    },

    onTreeNodeSelect: function (cell, record, item, index, e, eOpts) {
        var clientTree = cell.view.panel.up('clienttree');
        var form = clientTree.down('editorform');
        var grid = cell.view.panel,
            addButton = clientTree.down('#addNode'),
            deleteButton = clientTree.down('#deleteNode'),
            updateButton = clientTree.down('#updateNode'),
            moveButton = clientTree.down('#moveNode');
        var buttonsExist = updateButton && deleteButton && moveButton;
        var needEnable = buttonsExist && deleteButton.disabled && updateButton.disabled && moveButton.disabled;
        if (record.isRoot()) {
            this.enableButtons([addButton]);
            this.disableButtons([updateButton, deleteButton, moveButton]);
        }
        else if (record.get('EndDate') != null) {
            this.disableButtons([updateButton, deleteButton, addButton]);
        }
        else /*if (needEnable)*/ {
            this.enableButtons([updateButton, deleteButton, addButton]);
        }
        if (form) {
            this.updateTreeDetail(form, record);
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
        this.setLogoImage(editorform.up('clienttree').down('[name=treeLogoPanel]'), record.data.LogoFileName);
    },

    // ClientTree
    onTreeAfterrender: function (tree, e) {
        var treegrid = tree.down('basetreegrid'),
            promoeditorcustom = treegrid.up('promoeditorcustom'),
            promocontainer = treegrid.up('#promo'),
            store = treegrid.getStore(),
            me = this,
            deleteButton = treegrid.down('#deleteNode'),
            updateButton = tree.down('#updateNode'),
            moveButton = treegrid.down('#moveNode'),
            proxy = store.getProxy();

        proxy.extraParams.filterParameter = null;
        proxy.extraParams.needBaseClients = true;
        me.disableButtons([updateButton, deleteButton, moveButton]);


        //так как автозагрузка стора отключена, необходимо загрузить стор, если иерархия открыта не в окне промо или в окне промо в режиме создания
        if (tree.needLoadTree && (!promocontainer || (promoeditorcustom && promoeditorcustom.isCreating) && !promoeditorcustom.isFromSchedule)) {
            proxy.extraParams.needBaseClients = false;
            store.load();
        }

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

    setCheckTree: function (nodes, checked, checkedArray) {
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
					if (node.get('EndDate') == null) {
						node.set('checked', c);
					}
				} else {
					var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
					if (promoeditorcustom) {
						if (node.get('IsBaseClient') && node.get('EndDate') == null) {
							node.set('checked', checked);
						}
					} else {
						node.set('checked', checked);
					}
                }

                if (node.get('IsBaseClient')) {
                    var nodeHtml = node.getOwnerTree().getView().getNode(node);
                    if (nodeHtml)
                        Ext.fly(nodeHtml).addCls('hierarchy-baseclient');
                }

                me.setCheckTree(node.childNodes, checked, checkedArray);
            });
        }
    },

    onUpdateButtonClick: function (button) {
        var tree = button.up('clienttree').down('clienttreegrid'),
            store = tree.getStore(),
            selModel = tree.getSelectionModel(),
            me = this,
            form = button.up('clienttree').down('editorform');

        if (selModel.hasSelection()) {
            var record = selModel.getSelection()[0];
            var model = Ext.create(Ext.ModelManager.getModel(store.model));

            tree.editorModel.startEditRecord(record);

            var clientTreeEditorWindow = Ext.ComponentQuery.query('clienttreeeditor')[0],
                srch = clientTreeEditorWindow.down('searchcombobox');

            clientTreeEditorWindow.down('#ok').on('click', function () {
                var scroll = $('#vScrollClientTree' + tree.id).data('jsp');

                tree.lastScrollHeight = scroll.getContentHeight();
                tree.lastScrollY = scroll.getContentPositionY();
            });

            clientTreeEditorWindow.beforeDestroy = function () {
                selModel.select(record);
                me.updateTreeDetail(form, record);
            };

            if (record.data.DeviationCoefficient === null || record.data.DeviationCoefficient === 0) {
                record.data.DeviationCoefficient = 0;

                var adjustmentNumber = clientTreeEditorWindow.down('numberfield[name=Adjustment]');
                adjustmentNumber.setValue(0);
            }

            tree.createMode = false;
            srch.hide();
        } else {
            console.log('No selection');
        }
    },

    onDeleteNodeButtonClick: function (button) {
        var treegrid = button.up('combineddirectorytreepanel').down('basetreegrid'),
            record = treegrid.getSelectionModel().getSelection()[0],
			msg = l10n.ns('tpm', 'DeleteText').value('deleteNodeWarning'),
            me = this;

        Ext.Msg.show({
			title: l10n.ns('tpm', 'DeleteText').value('deleteWindowTitle'),
            msg: msg,
            fn: function (buttonId) {
                if (buttonId === 'yes') {
                    treegrid.setLoading(true);
                    var parameters = {
                        key: record.get("Id")
                    };

                    App.Util.makeRequestWithCallback('ClientTrees', 'Delete', parameters, function (data) {
                        var result = Ext.JSON.decode(data.httpResponse.data.value);
                        if (result.success) {
                            var targetId;

                            if (record.nextSibling) {
                                targetId = record.nextSibling.get('ObjectId');
                            }
                            else if (record.previousSibling) {
                                targetId = record.previousSibling.get('ObjectId');
                            }
                            else {
                                targetId = record.get('parentId');
                            }

                            treegrid.store.getProxy().extraParams.clientObjectId = targetId;
                            treegrid.store.getRootNode().removeAll();
                            treegrid.store.getRootNode().setId('root');
                            treegrid.store.load({
                                scope: this,
                                callback: function (records, operation, success) {
                                    if (success) {
                                        var choosenRecord = treegrid.store.getById(targetId);
                                        if (choosenRecord.parentNode.isExpanded()) {
                                            treegrid.getSelectionModel().select(choosenRecord);
                                            treegrid.fireEvent('itemclick', treegrid.getView(), choosenRecord);

                                            me.scrollNodeToCenterTree(treegrid, choosenRecord);
                                        }
                                    }
                                }
                            });
                            treegrid.setLoading(false);
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

    onBaseClientsCheckbox: function (checkBox, newValue, oldValue) {
        var clientTree = checkBox.up('clienttree');
        this.applyFiltersForTree(clientTree);
    },

    onAddNodeButtonClick: function (button) {
        var tree = button.up('clienttree').down('clienttreegrid');
        var store = tree.getStore();
        var model = Ext.create(Ext.ModelManager.getModel(store.model));

        tree.createMode = true;

        var selModel = tree.getSelectionModel();
        if (selModel.hasSelection()) {
            var record = selModel.getSelection()[0];
            // заполняем модель 
            model.set('parentId', record.get('ObjectId')); // id родительского элемента
            model.set('StartDate', new Date()); // дата, перепишется на сервер, но нужна т.к. иначе модель будет невалидна
            model.set('EndDate', null); // дата нужна, иначе модель будет невалидна !!!
            model.set('depth', record.get('depth') + 1); // глубина вложенности           

            tree.editorModel.startCreateRecord(model);

            var typesFilter = this.getTypesFilter(record, []);
            var clientTreeEditorWindow = Ext.ComponentQuery.query('clienttreeeditor')[0]
            var srch = clientTreeEditorWindow.down('searchcombobox'),
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
                                operation: 'GreaterThan',
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
        var me = this;
        if (success) {
            var treegrid = node.getOwnerTree(),
                gridView = treegrid.getView(),
                selModel = treegrid.getSelectionModel(),
                store = treegrid.getStore(),
                currentRoot = store.getRootNode(),
                trueRoot = store.getById('5000000'),
                clientTree = treegrid.up('clienttree');

            // workaround - не нашёл способа загружать дерево вместе с рутом с сервера
            var rootIsDefault = currentRoot.getId() != '5000000';
            if (rootIsDefault) {
                store.setRootNode(trueRoot);
            }

            me.checkForBase(treegrid.getView());

            var mainView = treegrid.up('clienttree'),
                form = mainView.down('editorform');
            var sels = selModel.getSelection();

            if (sels.length == 0 && form && records[0]) {
                if (records[0].get('ObjectId') == 5000000) {
                    selModel.select(records[0]);
                    treegrid.fireEvent('itemclick', gridView, records[0]);
                }
            }

            treegrid.unmask();

            if (treegrid.isDisabled()) {
                treegrid.mask();
                $('#' + treegrid.id + ' .x-mask').css('opacity', '0.1');
            }
            
            if (clientTree.chooseMode) {
                // разве одного любого чекнуть не достаточно?
                // если да, то иконки базового нужно будет переделать
                var nodes = node.childNodes.length !== 0 ? node.childNodes : records[0].childNodes.length !== 0 ? records[0].childNodes : null;
                me.setCheckTree(nodes, false);
                this.controllCheckClient(clientTree);
            } else if (treegrid.checkedNodes) {
                // для расширенного фильтра при выборе операции Список
                var nodes = node.childNodes.length !== 0 ? node.childNodes : records[0].childNodes.length !== 0 ? records[0].childNodes : null;
                me.setCheckTree(nodes, false, treegrid.checkedArray);
            }

            store.getProxy().extraParams.clientObjectId = null;
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

    onTreeNodeExpandCollapse: function (node) {
        var gridView = node.getOwnerTree().getView();
        this.checkForBase(gridView, node);
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

    // добавляем класс css для базовых клиентов
    checkForBase: function (gridView, node) {
        var me = this;
        var node = node || gridView.getTreeStore().getRootNode(); // начинаем с корня

        if (node.get('IsBaseClient')) {
            var nodeHtml = gridView.getNode(node);
            if (nodeHtml)
                Ext.fly(nodeHtml).addCls('hierarchy-baseclient');
        }

        if (node.isExpanded()) {
            Ext.each(node.childNodes, function (child) {
                me.checkForBase(gridView, child);
            });
        }
    },

    onCheckChange: function (node) {
        var gridView = node.getOwnerTree().getView();
        this.checkForBase(gridView);
    },

    setScroll: function (tree) {
        // замена скролла в дереве
        var treeHtml = $('#' + tree.id);
        var treeViewHtml = $('#' + tree.getView().id);
        var table = treeViewHtml.find('table');
        table.css('width', 'initial');

        var heightScroll = table.length > 0 ? table.height() : 0;
        var widthScroll = table.length > 0 ? table.innerWidth() : 0;
        var jspV = $('#vScrollClientTree' + tree.id);
        var jspH = $('#hScrollClientTree' + tree.id);

        // если скролла есть, то обновить, иначе создать
        if (jspV.length > 0) {
            jspV.height(treeViewHtml.height());
            $('#vScrollClientTreeDiv' + tree.id).height(heightScroll);
            jspV.data('jsp').reinitialise();
            $('#vScrollClientTree' + tree.id).data('jsp').scrollToY(treeViewHtml.scrollTop());
        } else {
            treeViewHtml.css('overflow', 'hidden');
            treeHtml.append('<div id="vScrollClientTree' + tree.id +'" class="vScrollTree scrollpanel" style="height: ' + treeViewHtml.height() + 'px;">'
                + '<div id="vScrollClientTreeDiv' + tree.id +'" style="height: ' + heightScroll + 'px;"></div></div>');

            $('#vScrollClientTree' + tree.id).jScrollPane();
            $('#vScrollClientTree' + tree.id).data('jsp').scrollToY(treeViewHtml.scrollTop());
            $('#vScrollClientTree' + tree.id).on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
                treeViewHtml.scrollTop(scrollPositionY);
                return false;
            });
        }

        if (jspH.length > 0) {
            jspH.width(treeViewHtml.width());
            $('#hScrollClientTreeDiv' + tree.id).width(widthScroll);
            jspH.data('jsp').reinitialise();
            $('#hScrollClientTree' + tree.id).data('jsp').scrollToX(treeViewHtml.scrollLeft());
        } else {
            treeHtml.append('<div id="hScrollClientTree' + tree.id +'" class="hScrollTree scrollpanel">'
                + '<div id="hScrollClientTreeDiv' + tree.id +'" style="width: ' + widthScroll + 'px;"></div></div>');

            $('#hScrollClientTree' + tree.id).jScrollPane();
            $('#hScrollClientTree' + tree.id).data('jsp').scrollToX(treeViewHtml.scrollLeft());
            $('#hScrollClientTree' + tree.id).on('jsp-scroll-x', function (event, scrollPositionX, isAtTop, isAtBottom) {
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
            var currentHeightScroll = $('#vScrollClientTreeDiv' + tree.id).height();
            var currentWidthScroll = $('#hScrollClientTreeDiv' + tree.id).width();

            if ((heightTable && heightTable != 0 && currentHeightScroll && heightTable != currentHeightScroll)
                || (widthTable && widthTable != 0 && currentWidthScroll && widthTable != currentWidthScroll))
                me.setScroll(tree);

            return false;
        });

        $('#' + tree.getView().id).on('wheel', function (e) {
            var direction = e.originalEvent.deltaY > 0 ? 1 : -1;
            var scrollValue = this.scrollTop + direction * 40;

            $('#vScrollClientTree' + tree.id).data('jsp').scrollToY(scrollValue);
            return false;
        });

        $('#' + tree.getView().id).on('scroll', function (e) {
            if (this.scrollTop != $('#vScrollClientTree' + tree.id).scrollTop())
                $('#vScrollClientTree' + tree.id).data('jsp').scrollToY(this.scrollTop);
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

        var clientTree = button.up('clienttree');
        var okButton = Ext.ComponentQuery.query('button[action="ok"]')[0];
        var dateFilterButton = Ext.ComponentQuery.query('#dateFilter')[0];
        var clientTreeGrid = clientTree.down('clienttreegrid');
        var store = clientTreeGrid.store;

        okButton.addListener('click', function () {
            var resultDate = datetimeField.getValue();
            var days = resultDate.getDate().toString().length === 1 ? '0' + resultDate.getDate() : resultDate.getDate();
            var month = (resultDate.getMonth() + 1).toString().length === 1 ? '0' + (resultDate.getMonth() + 1) : resultDate.getMonth() + 1;
            var year = resultDate.getFullYear();
            button.dateValue = resultDate;
            dateFilterButton.setText(days + '.' + month + '.' + year);
            store.getProxy().extraParams.dateFilter = resultDate;
            me.applyFiltersForTree(clientTree);
        });
    },

    onClientTreeEditorClose: function (window) {
       // debugger;
        var gridView = Ext.ComponentQuery.query('clienttreegrid')[0].getView();
        this.checkForBase(gridView);
    },

    onBaseTreeGridDisable: function (grid) {
        var toolbar = grid.up('clienttree').down('customtoptreetoolbar');

        toolbar.down('#clientsSearchTrigger').setDisabled(true);
        toolbar.down('#baseClientsCheckbox').setDisabled(true);
    },

    onBaseTreeGridEnable: function (grid) {
        var toolbar = grid.up('clienttree').down('customtoptreetoolbar');

        toolbar.down('#clientsSearchTrigger').setDisabled(false);
        toolbar.down('#baseClientsCheckbox').setDisabled(false);
    },

    onClientTreeEditorAfterRender: function (clientTreeEditor) {
        var clientTreeGrid = Ext.ComponentQuery.query('clienttreegrid')[0];
        var isBaseClientCombobox = clientTreeEditor.down('[name=IsBaseClient]');
        var distrMarkUp = clientTreeEditor.down('[name=DistrMarkUp]');

        var val = isBaseClientCombobox.getRawValue();

        if (val == "Yes") {
            distrMarkUp.setDisabled(false);
        } else {
            distrMarkUp.setDisabled(true);
        }

        isBaseClientCombobox.addListener('select', function (combo) {
            if (combo.getRawValue() === 'Yes') {
                var myMask = new Ext.LoadMask(clientTreeEditor, { msg: "Please wait..." })
                myMask.show();

                var parameters = {
                    objectId: clientTreeGrid.getSelectionModel().getSelection()[0].data.ObjectId,
                    isCreateMode: clientTreeGrid.createMode
                };

                if (clientTreeGrid.getSelectionModel().getSelection()[0].data.IsBaseClient && !clientTreeGrid.createMode) {
                    myMask.hide();
                } else {
                    App.Util.makeRequestWithCallback('ClientTrees', 'CanCreateBaseClient', parameters, function (data) {
                        var result = Ext.JSON.decode(data.httpResponse.data.value);

                        if (result.success) {
                            myMask.hide();
                        } else {
                            isBaseClientCombobox.setValue(false);
                            distrMarkUp.setDisabled(true);
                            App.Notify.pushInfo('The current branch already contains the base client.');
                            myMask.hide();
                        }
                    },
                        function () {
                            isBaseClientField.setValue(false);
                            App.Notify.pushError(data.message);
                            myMask.hide();
                        })
                }
                distrMarkUp.setDisabled(false);
            } else {
                distrMarkUp.setDisabled(true);
                distrMarkUp.setValue(null);
            }

        });
    },

    checkRolesAccess: function (grid) {
        var clientTree = grid.up('clienttree');
        var resource = clientTree.getBaseModel().getProxy().resourceName;
        var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;

        // кнопки которые проверяем на доступ
        var btns = ['#addNode', '#deleteNode', '#updateNode', '#attachFile', '#deleteAttachFile', '#attachFileName'];

        Ext.each(btns, function (btnName) {
            var btn = clientTree.down(btnName);
            var access = pointsAccess.find(function (element) {
                return element.Resource == resource && element.Action == btn.action;
            });

            if (!access)
                btn.setVisible(false);
        });
    },

    onClientTextSearch: function (field, e) {
        if (!e || e.getKey() == e.ENTER) {
            var clientTree = field.up('clienttree');
            this.applyFiltersForTree(clientTree);
        }
    },

    applyFiltersForTree: function (clientTree) {        
        var textFieldSearch = clientTree.down('#clientsSearchTrigger');
        var baseClientChBox = clientTree.down('#baseClientsCheckbox');
        var store = clientTree.down('basetreegrid').store;
        var proxy = store.getProxy();
        var me = this;
        var selectionWidget = clientTree.up('#associatedbudgetsubitemclienttree_clienttree_selectorwindow');

        var textSearch = textFieldSearch.getValue()
        if (textSearch && textSearch.length > 0 && textSearch.indexOf('Client search') == -1)
            proxy.extraParams.filterParameter = textSearch;

        proxy.extraParams.needBaseClients = baseClientChBox.getValue();
                
        if (clientTree.choosenClientObjectId)
            proxy.extraParams.clientObjectId = clientTree.choosenClientObjectId;

        if (selectionWidget && selectionWidget.budgetSubItemId) {
            proxy.extraParams.budgetSubItemId = selectionWidget.budgetSubItemId
        }

        store.getRootNode().removeAll();
        store.getRootNode().setId('root');
        store.load();

        proxy.extraParams.filterParameter = null;
        proxy.extraParams.needBaseClients = false;
        proxy.extraParams.clientObjectId = null;
        proxy.extraParams.budgetSubItemId = null;
    },

    // следит за галочками
    controllCheckClient: function (clientTree) {        
        var clientTreeGrid = clientTree.down('basetreegrid');
        var store = clientTreeGrid.store;
        var proxy = store.getProxy();
        var targerNodeId;

        if (proxy.extraParams.clientObjectId != null)
            targerNodeId = proxy.extraParams.clientObjectId;
        else
            targerNodeId = clientTree.choosenClientObjectId;

        var targetnode = store.getNodeById(targerNodeId);

        if (targetnode) {
            var selectionModel = clientTreeGrid.getSelectionModel();
            var countChecked = clientTreeGrid.getChecked().length;

            clientTreeGrid.fireEvent('checkchange', targetnode, true);

            if (clientTreeGrid.isDisabled()) {
                selectionModel.deselectAll();
            }
            else if (countChecked == 0 && targetnode.parentNode.isExpanded()) {
                selectionModel.select(targetnode);
                this.scrollNodeToCenterTree(clientTreeGrid, targetnode);
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

    onIsBeforeStartChange: function (field, newValue, oldValue) {
        var isDaysStart = field.up('container').down('[name=IsDaysStart]');
        var daysStart = field.up('container').down('[name=DaysStart]');
        var record = field.up('form').getRecord();

        if (record) {
            if (newValue === true || newValue === false) {
                isDaysStart.setDisabled(false);
                daysStart.setDisabled(false);
            } else {
                isDaysStart.reset();
                daysStart.reset();
                isDaysStart.setDisabled(true);
                daysStart.setDisabled(true);
                if (field.up('window').isVisible()) { //change срабатывает после закрытия окна и отмены изменений записи. Если окно закрыто - не надо устанавливать значения
                    record.set('IsDaysStart', null);
                    record.set('DaysStart', null);
                }
            }
        }
    },

    onIsBeforeEndChange: function (field, newValue, oldValue) {
        var isDaysEnd = field.up('container').down('[name=IsDaysEnd]');
        var daysEnd = field.up('container').down('[name=DaysEnd]');
        var record = field.up('form').getRecord();
        if (record) {
            if (newValue === true || newValue === false) {
                isDaysEnd.setDisabled(false);
                daysEnd.setDisabled(false);
            } else {
                isDaysEnd.reset();
                daysEnd.reset();
                isDaysEnd.setDisabled(true);
                daysEnd.setDisabled(true);
                if (field.up('window').isVisible()) {
                    record.set('IsDaysEnd', null);
                    record.set('DaysEnd', null);
                }
            }
        }
    },


    onAttachFileButtonClick: function (button) {
        var resource = 'ClientTrees';
        var action = 'UploadLogoFile';

        var uploadFileWindow = Ext.widget('uploadfilewindow', {
            itemId: 'clientTreeUploadFileWindow',
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
        var clientTree = button.up('clienttree');
        var logoPanel = clientTree.down('[name=treeLogoPanel]');
        var currentNode = clientTree.down('clienttreegrid').getSelectionModel().getSelection()[0];

        var parameters = {
            id: currentNode.get('Id')
        };

        App.Util.makeRequestWithCallback('ClientTrees', 'DeleteLogo', parameters,
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
        var clientTree = Ext.ComponentQuery.query('clienttree')[0];
        var logoPanel = clientTree.down('[name=treeLogoPanel]');
        var win = button.up('uploadfilewindow');
        var currentNode = clientTree.down('clienttreegrid').getSelectionModel().getSelection()[0];
        var url = Ext.String.format("/odata/{0}/{1}?clientTreeId={2}", win.resource, win.action, currentNode.get('Id'));
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
                        var pattern = '/odata/ClientTrees/DownloadLogoFile?fileName={0}';
                        var downloadFileUrl = document.location.href + Ext.String.format(pattern, o.result.fileName);

                        var attachFileName = Ext.ComponentQuery.query('#attachFileName')[0];
                        attachFileName.setValue('<a href=' + downloadFileUrl + '>' + o.result.fileName + '</a>');
                        attachFileName.attachFileName = o.result.fileName;

                        var currentNode = Ext.ComponentQuery.query('clienttreegrid')[0].getSelectionModel().getSelection()[0];
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
            var pattern = '/odata/ClientTrees/DownloadLogoFile?fileName={0}';
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
