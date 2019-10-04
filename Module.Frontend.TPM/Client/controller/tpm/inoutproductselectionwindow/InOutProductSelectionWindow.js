Ext.define('App.controller.tpm.inoutselectionproductwindow.InOutSelectionProductWindow', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'inoutselectionproductwindow #ok': {
                    click: this.onOkButtonClick
                },
                'inoutselectionproductwindow #productsSearchTrigger': {
                    specialkey: this.onProductTextSearch,
                    applySearch: this.applyFiltersForTree
                },
                'inoutselectionproductwindow producttreegrid': {
                    afterrender: this.onProductTreeGridAfterRender,
                    checkchange: this.onProductTreeCheckChange
                },
                'inoutselectionproductwindow product #excludeassortmentmatrixproductsbutton': {
                    afterrender: this.onExcludeAssortimentMatrixProductsAfterRender,
                    toggle: this.onExcludeAssortimentMatrixProductsButtonToggle
                },
                'inoutselectionproductwindow product': {
                    afterrender: this.onProductAfterRender
                },
                'inoutselectionproductwindow product directorygrid': {
                    beforerender: this.productGridBeforeRender,
                    afterrender: this.onProductGridAfterRender,
                    selectionchange: this.onProductGridSelectionChange
                },
                'inoutselectionproductwindow product directorygrid gridcolumn[cls=select-all-header]': {
                    headerclick: this.onSelectAllRecordsClick
                },
                'inoutselectionproductwindow #dateFilter': {
                    click: this.onDateFilterButtonClick
                },
            }
        });
    },

    // Кнопка подтверждения выбора продуктов
    onOkButtonClick: function (button) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var productTreeGrid = button.up('window').down('producttreegrid');
        var product = button.up('window').down('product');
        var productGrid = product.down('grid');
        var productGridStore = productGrid.getStore();
        var excludeAssortmentMatrixProductsButton = product.down('#excludeassortmentmatrixproductsbutton');
        var checkedRecords = productGrid.getSelectionModel().getCheckedRows();
        var promoController = App.app.getController('tpm.promo.Promo');
        var productTreeCheckedNodes = productTreeGrid.getChecked();

        if (productGridStore.getTotalCount() > 0) {
            // Все продкуты из стора
            var productRecords = productGridStore.getRange(0, productGridStore.getTotalCount());
            // Все выбранные продукты, включая те, что отсеились по фильтрам, матрице.
            var checkedRecords = productGrid.getSelectionModel().getCheckedRows();
            // Только те выбранные, что видит пользователь.
            var checkedProductsInGrid = productRecords.filter(function (record) {
                return checkedRecords.some(function (checkedRecord) { return record.data.Id == checkedRecord.data.Id })
            });

            if (checkedProductsInGrid.length != 0) {
                // Хранит выбранные продукты через ;
                promoEditorCustom.InOutProductIds = inOutSelectionProductWindowController.parseRecordsToString(checkedProductsInGrid);
                // Хранит исключенные продукты через ;
                //promoEditorCustom.RegularExcludedProductIds = inOutSelectionProductWindowController.getRegularExcludedProductsString(productRecords, checkedProductsInGrid);
                // Хранит инфу о продуктовой иерархии
                promoEditorCustom.productTreeNodes = productTreeCheckedNodes.slice();
                promoEditorCustom.productHierarchy = productTreeCheckedNodes[productTreeCheckedNodes.length - 1].data.FullPathName;

                var promoProductsForm = promoEditorCustom.down('promobasicproducts');
                promoProductsForm.choosenProductObjectIds = productTreeCheckedNodes;
                // Нужно передать массив из последнего / последних узлов.
                promoProductsForm.fillForm(inOutSelectionProductWindowController.getCheckedProductTreeLeafsArray(productTreeGrid.getChecked()));
                promoController.setInfoPromoBasicStep2(promoProductsForm);

                promoEditorCustom.excludeAssortmentMatrixProductsButtonPressed = excludeAssortmentMatrixProductsButton.pressed;
                button.up('window').close();
            } else {
                App.Notify.pushInfo(l10n.ns('tpm', 'InOutProductSelectionWindow').value('productrequired'));
            }
        } else {
            App.Notify.pushInfo(l10n.ns('tpm', 'InOutProductSelectionWindow').value('productrequired'));
        }
    },

    onProductTreeGridAfterRender: function (productTreeGrid) {
        var me = this;
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var productTreeGridStore = productTreeGrid.getStore();
        var inOutProductSelectionWindowProductTreeGridContainer = productTreeGrid.up('#InOutProductSelectionWindowProductTreeGridContainer');
        var inOutSelectionProductWindow = productTreeGrid.up('inoutselectionproductwindow');
        var promoBasicProductsRecord = Ext.ComponentQuery.query('promobasicproducts')[0].promoProductRecord;
        var productTreeGridStoreProxy = productTreeGridStore.getProxy();
        var excludeassortmentmatrixproductsbutton = productTreeGrid.up('window').down('#excludeassortmentmatrixproductsbutton');

        // Если открыли сохраненное промо.
        if (promoBasicProductsRecord && promoBasicProductsRecord.ProductsChoosen.length > 0) {
            var productObjectIds = '';
            promoBasicProductsRecord.ProductsChoosen.forEach(function (product) {
                productObjectIds += product.ObjectId + ';';
            });
            productTreeGridStoreProxy.extraParams.productTreeObjectIds = productObjectIds;
        }

        inOutProductSelectionWindowProductTreeGridContainer.setLoading(true);
        productTreeGridStore.addListener('load', function () {
            var rootNode = productTreeGrid.getRootNode().childNodes[0];
            var productsRootNodeObjectId = rootNode.data.ObjectId;

            if (productsRootNodeObjectId == '1000000') {
                productTreeGrid.setRootNode(rootNode, false);
                productTreeGrid.getRootNode().expand();
            }

            var targetNodes = productTreeGrid.getChecked();
            me.getProductTreeController().setCheckTree(productTreeGrid.getRootNode().childNodes, false, targetNodes);
            me.controllCheckProduct(productTreeGrid, targetNodes);

            productTreeGridStoreProxy.extraParams.productTreeObjectIds = null;
            excludeassortmentmatrixproductsbutton.fireEvent('toggle', excludeassortmentmatrixproductsbutton, excludeassortmentmatrixproductsbutton.pressed);

            var grid = Ext.ComponentQuery.query('product')[0].down('grid');
            inOutSelectionProductWindowController.setInfoGridToolbar(grid);
        });

        var dateFilter = inOutSelectionProductWindow.down('#dateFilter');
        var currentDate = new Date();

        var days = currentDate.getDate().toString().length === 1 ? '0' + currentDate.getDate() : currentDate.getDate();
        var month = (currentDate.getMonth() + 1).toString().length === 1 ? '0' + (currentDate.getMonth() + 1) : currentDate.getMonth() + 1;
        var year = currentDate.getFullYear();

        if (dateFilter) {
            dateFilter.setText(days + '.' + month + '.' + year);
        }

        productTreeGridStore.load();
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

        var productTreeGrid = Ext.ComponentQuery.query('producttreegrid')[0];
        var okButton = Ext.ComponentQuery.query('button[action="ok"]')[0];
        var dateFilterButton = Ext.ComponentQuery.query('#dateFilter')[0];
        var store = productTreeGrid.store;

        okButton.addListener('click', function () {
            var resultDate = datetimeField.getValue();
            var days = resultDate.getDate().toString().length === 1 ? '0' + resultDate.getDate() : resultDate.getDate();
            var month = (resultDate.getMonth() + 1).toString().length === 1 ? '0' + (resultDate.getMonth() + 1) : resultDate.getMonth() + 1;
            var year = resultDate.getFullYear();
            button.dateValue = resultDate;
            dateFilterButton.setText(days + '.' + month + '.' + year);
            store.getProxy().extraParams.dateFilter = resultDate;
            me.applyFiltersForTree(productTreeGrid);
        });
    },

    onProductTextSearch: function (field, e) {
        if (!e || e.getKey() == e.ENTER) {
            var productTreeGrid = field.up('#InOutProductSelectionWindowProductTreeGridContainer').down('producttreegrid');
            this.applyFiltersForTree(productTreeGrid);
        }
    },

    applyFiltersForTree: function (productTreeGrid) {
        var inOutProductSelectionWindowProductTreeGridContainer = productTreeGrid.up('#InOutProductSelectionWindowProductTreeGridContainer');
        inOutProductSelectionWindowProductTreeGridContainer.setLoading(true);

        var textFieldSearch = inOutProductSelectionWindowProductTreeGridContainer.down('#productsSearchTrigger');
        var store = productTreeGrid.getStore();
        var proxy = store.getProxy();

        var textSearch = textFieldSearch.getValue()
        if (textSearch && textSearch.length > 0 && textSearch.indexOf('Product search') == -1)
            proxy.extraParams.filterParameter = textSearch;

        if (inOutProductSelectionWindowProductTreeGridContainer.length > 0) {
            inOutProductSelectionWindowProductTreeGridContainer.forEach(function (objectId, index) {
                proxy.extraParams.productTreeObjectIds += objectId;

                if (index != inOutProductSelectionWindowProductTreeGridContainer.length - 1)
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

    onProductTreeCheckChange: function (item, checked, eOpts) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var store = item.store;
        var treegrid = store.ownerTree;
        var nodes = treegrid.getRootNode().childNodes;
        var c = this.getController('tpm.product.ProductTree');
        var inOutProductSelectionWindowProductTreeGridContainer = treegrid.up('#InOutProductSelectionWindowProductTreeGridContainer');
        var product = treegrid.up('inoutselectionproductwindow').down('product');
        var productGrid = product.down('grid');
        var productGridSelectionModel = productGrid.getSelectionModel();
        var productGridStore = productGrid.getStore();

        // если узел имеет тип subrange, то ищем отмеченные узлы на том же уровне        
        var multiCheck = false;

        if (item.get('Type').indexOf('Subrange') >= 0) {
            for (var i = 0; i < item.parentNode.childNodes.length && !multiCheck; i++) {
                var node = item.parentNode.childNodes[i];
                multiCheck = node !== item && node.get('checked') && node.get('Type').indexOf('Subrange') >= 0;
            }
        }
        // если нельзя отметить несколько, сбрасываем галочки
        if (!multiCheck) {
            c.setCheckTree(nodes, false);
            inOutProductSelectionWindowProductTreeGridContainer.choosenProductObjectId = [];
        }

        if (checked) {
            item.set('checked', true);
            inOutProductSelectionWindowProductTreeGridContainer.choosenProductObjectId.push({
                ObjectId: item.get('ObjectId'),
                Filter: item.get('Filter')
            });

            var parent = store.getNodeById(item.get('parentId'));
            if (parent) {
                while (parent.data.root !== true) {
                    if (parent.get('Type').indexOf('Brand') < 0) {
                        parent.set('checked', true);
                    }
                    parent = store.getNodeById(parent.get('parentId'));
                }
            }
        }
        else {
            item.set('checked', false);

            var indexObjectId = -1;
            for (var i = 0; i < inOutProductSelectionWindowProductTreeGridContainer.choosenProductObjectId.length; i++) {
                if (inOutProductSelectionWindowProductTreeGridContainer.choosenProductObjectId[i].ObjectId == item.data.ObjectId) {
                    indexObjectId = i;
                    break;
                }
            }

            if (indexObjectId >= 0) {
                inOutProductSelectionWindowProductTreeGridContainer.choosenProductObjectId.splice(indexObjectId, 1);
            }
        }

        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0]; 

        // Если промо регулярное, отметить все продукты при выборе узла.
        if (!promoEditorCustom.isInOutPromo && checked) {
            productGridStore.on({
                load: {
                    fn: function () {
                        if (!promoEditorCustom.InOutProductIds && !(promoEditorCustom.model && promoEditorCustom.model.data.InOutProductIds)) {
                            var productGridSelectionModel = productGrid.getSelectionModel();
                            productGridSelectionModel.selectAllRecords(productGrid.down('gridcolumn[cls=select-all-header]'));
                            productGridSelectionModel.allSelected = true;

                            inOutSelectionProductWindowController.setInfoGridToolbar(productGrid);
                        }
                    },
                    single: true
                },
            });
        }

        var treegridChecked = treegrid.getChecked();
        var checkedLeafs = inOutSelectionProductWindowController.getCheckedProductTreeLeafsArray(treegridChecked);

        // Сохранить предыдущие выбранные узлы еирархии
        treegrid.previousProductTreeNodes = treegrid.currentProductTreeNodes || [];
        // Сохранить выбранные узлы иерархии
        treegrid.currentProductTreeNodes = checkedLeafs

        // Нужно ли сбросить выбранные продукты?
        var needResetCheckboxes = inOutSelectionProductWindowController.needResetCheckboxes(inOutSelectionProductWindowController, treegrid.previousProductTreeNodes, treegrid.currentProductTreeNodes);
        if (needResetCheckboxes && promoEditorCustom.isInOutPromo) {
            productGridSelectionModel.uncheckRows(productGridSelectionModel.getCheckedRows());
        }

        if (treegrid.getChecked() > 0) {
            productGridStore.removeFixedFilter('InOutProductSelectionWindow');
        }

        inOutSelectionProductWindowController.setInfoGridToolbar(productGrid);
        var excludeassortmentmatrixproductsbutton = product.down('#excludeassortmentmatrixproductsbutton');
        excludeassortmentmatrixproductsbutton.fireEvent('toggle', excludeassortmentmatrixproductsbutton, excludeassortmentmatrixproductsbutton.pressed);
    },

    controllCheckProduct: function (productTreeGrid, targetNodes) {
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
            }
        }
    },

    // Добавляет галки в грид
    productGridBeforeRender: function (grid) {
        var productGridStore = grid.getStore();
        var promoBasicProductsRecord = Ext.ComponentQuery.query('promobasicproducts')[0].promoProductRecord;

        // Если есть выбранные продукты.
        if (promoBasicProductsRecord && promoBasicProductsRecord.ProductsChoosen.length > 0) {
            productGridStore.removeFixedFilter('InOutProductSelectionWindow');
        } else {
            productGridStore.setFixedFilter('InOutProductSelectionWindow', { operator: "and", rules: [{ property: "Id", operation: "Equals", value: null }] });
        }

        grid.multiSelect = true;
    },

    // Получить продуктовый контроллер
    getProductTreeController: function () {
        return App.app.getController('tpm.product.ProductTree');
    },

    onProductEditorAfterRender: function (window) {
        // Скрыть кнопку редактирования продуктов
        window.down('#edit').hide();
    },

    onExcludeAssortimentMatrixProductsAfterRender: function (button) {
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];

        if (!promoEditorCustom.excludeAssortmentMatrixProductsButtonPressed
            ? (promoEditorCustom.model && promoEditorCustom.model.InOutExcludeAssortmentMatrixProductsButtonPressed) : promoEditorCustom.excludeAssortmentMatrixProductsButtonPressed) {
            button.toggle();
        }
    },

    onExcludeAssortimentMatrixProductsButtonToggle: function (button, isPressed) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var window = button.up('window');
        var inOutProductSelectionWindowProductTreeGridContainer = window.down('#InOutProductSelectionWindowProductTreeGridContainer');
        var product = window.down('product');
        var productGrid = product.down('grid');
        var productGridSelectionModel = productGrid.getSelectionModel();
        var productGridStore = productGrid.getStore();
        var productGridStoreProxy = productGridStore.getProxy();
        var toolBar = window.down('custombigtoolbar');
        var productTreeGrid = window.down('producttreegrid');

        if (isPressed) {
            button.addCls('in-out-product-selection-window-product-toggled-button ');
            button.removeCls('in-out-product-selection-window-product-untoggled-button ');
        } else {
            button.addCls('in-out-product-selection-window-product-untoggled-button ');
            button.removeCls('in-out-product-selection-window-product-toggled-button ');
        }

        if (inOutProductSelectionWindowProductTreeGridContainer.choosenProductObjectId.length > 0) {
            inOutProductSelectionWindowProductTreeGridContainer.setLoading(true);
            toolBar.down('#excludeassortmentmatrixproductsbutton').setDisabled(true);

            productGridStore.addListener('load', function () {
                inOutProductSelectionWindowProductTreeGridContainer.setLoading(false);
                toolBar.down('#excludeassortmentmatrixproductsbutton').setDisabled(false);
            });

            var productTreeGridCheckedNodes = productTreeGrid.getChecked();
            if (productTreeGridCheckedNodes.length > 0) {
                productGridStore.removeFixedFilter('InOutProductSelectionWindow');

                var productTreeObjectIds = inOutSelectionProductWindowController.getCheckedProductTreeLeafsString(productTreeGridCheckedNodes);

                // Применить фильтры узлов && исключить продукты из ассортиментной матрицы.
                if (isPressed) {
                    productGridStoreProxy.extraParams.inOutProductTreeObjectIds = productTreeObjectIds;
                    productGridStoreProxy.extraParams.needInOutFilteredProducts = true;
                    productGridStoreProxy.extraParams.needInOutExcludeAssortmentMatrixProducts = true;
                    productGridStoreProxy.extraParams.needInOutSelectedProducts = false;
                    productGridStoreProxy.extraParams.inOutProductIdsForGetting = '';
                }
                // Применить только фильтры узлов. 
                else {
                    productGridStoreProxy.extraParams.inOutProductTreeObjectIds = productTreeObjectIds;
                    productGridStoreProxy.extraParams.needInOutFilteredProducts = true;
                    productGridStoreProxy.extraParams.needInOutExcludeAssortmentMatrixProducts = false;
                    productGridStoreProxy.extraParams.needInOutSelectedProducts = false;
                    productGridStoreProxy.extraParams.inOutProductIdsForGetting = '';
                }

                productGridStore.removeAll();
                productGridStore.load();
            }
        }
        // Сбросить фильтр.
        else {
            inOutProductSelectionWindowProductTreeGridContainer.setLoading(false);

            productGridStoreProxy.extraParams.inOutProductTreeObjectIds = '';
            productGridStoreProxy.extraParams.needInOutFilteredProducts = false;
            productGridStoreProxy.extraParams.needInOutExcludeAssortmentMatrixProducts = false;
            productGridStoreProxy.extraParams.needInOutSelectedProducts = false;
            productGridStoreProxy.extraParams.inOutProductIdsForGetting = '';

            productGridStore.setFixedFilter('InOutProductSelectionWindow', { operator: "and", rules: [{ property: "Id", operation: "Equals", value: null }] });
        }
    },

    getProductIdsList: function (inOutProductIds) {
        if (inOutProductIds) {
            return inOutProductIds.split(';');
        }

        return [];
    },

    onProductGridAfterRender: function (productGrid) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var me = this;
        var productGridStore = productGrid.getStore();
        var productTreeGrid = productGrid.up('window').down('producttreegrid');
        var productTreeGridStore = productGrid.up('window').down('producttreegrid').getStore();
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var promoModel = promoEditorCustom.model;
        var productGridSelectionModel = productGrid.getSelectionModel();
        var isFirstOpening = true;
        var nodeChanged = false;
        var excludeAssortmentMatrixProductsButton = productGrid.up('product').down('#excludeassortmentmatrixproductsbutton');
        var inOut = promoEditorCustom.isInOutPromo || (promoEditorCustom.model && promoEditorCustom.model.data.InOut);

        if (inOut) {
            excludeAssortmentMatrixProductsButton.show();
        } else {
            excludeAssortmentMatrixProductsButton.hide();
        }

        productGrid.getStore().addListener('prefetch', function (store, records) {
            me.setInfoGridToolbar(productGrid);
        });

        var firstProductTreeGridStore = function () {
            var productIdsForChecking = promoEditorCustom.InOutProductIds || (promoEditorCustom.model && promoEditorCustom.model.data.InOutProductIds);
            var productIdsForCheckingArray = inOutSelectionProductWindowController.getProductIdsList(productIdsForChecking);
            inOutSelectionProductWindowController.checkProductsByProductIds(productGrid, productIdsForCheckingArray, productTreeGrid, promoEditorCustom);
        };

        var maxStepsCount = 4;
        var stepCounter = 0;

        // Создает событие, которое должно вызваться при первом появлении данных в гриде.
        var maxStepsCount = 4;
        var createFirstProductTreeGridStoreEvent = function () {
            productGridStore.on('load', function (store) {
                if (store.getTotalCount() == 0 && stepCounter < maxStepsCount) {
                    stepCounter++;
                    createFirstProductTreeGridStoreEvent();
                } else {
                    firstProductTreeGridStore();
                }
            }, this, { single: true });
        }
        createFirstProductTreeGridStoreEvent();

        productGridStore.addListener('load', function (store, records) {
            inOutSelectionProductWindowController.setInfoGridToolbar(productGrid);
            // Установить актуальные выбраныне продукты
            inOutSelectionProductWindowController.removeHiddenRowsFromSelectionModel(productGridSelectionModel, productGrid);
        });
    },

    // Возвращает массив из выбранных листов продуктового дерева.
    getCheckedProductTreeLeafsArray: function (checkedNodes) {
        var checkedLeafs = [];

        if (checkedNodes.length > 0) {
            // Если в выбранных узлах есть Subrange, значит могут быть выбраны только узлы типа Subrange.
            if (checkedNodes.some(function (checkedNode) { return checkedNode.data.Type == 'Subrange' })) {
                // Перебираем выбранные Subranges и формируем результирующий массив.
                checkedNodes.filter(function (checkedNode) { return checkedNode.data.Type == 'Subrange' }).forEach(function (checkedNode) {
                    checkedLeafs.push(checkedNode);
                });
            }
            // Если в выбранные нет типа Subrange, значит можно взять последний выбранный в дереве.
            else {
                checkedLeafs.push(checkedNodes[checkedNodes.length - 1]);
            }
        }

        return checkedLeafs;
    },

    // Возвращает выбранные листы продуктового дерева через точку с запятой.
    getCheckedProductTreeLeafsString: function (checkedNodes) {
        var checkedLeafs = this.getCheckedProductTreeLeafsArray(checkedNodes);
        var checkedLeafsString = '';

        checkedLeafs.forEach(function (chechedLeaf) {
            checkedLeafsString += chechedLeaf.data.ObjectId + ';';
        });

        return checkedLeafsString;
    },

    // Все ли элементы первого массива включены во второй && равно ли количество элементов.
    compareArrays: function (firstArray, secondArray) {
        var result = true;

        if (firstArray.length == secondArray.length) {
            result = firstArray.every(function (x) {
                return secondArray.indexOf(x) > -1;
            });
        } else {
            result = false;
        }

        return result;
    },

    // Являются ли все узлы типом Subrange
    allNodesAreSubranges: function (nodes) {
        return nodes.every(function (node) {
            return node.data.Type == 'Subrange';
        });
    },

    // Получить ObjectId родителя узла
    getNodeParentObjectId: function (node) {
        if (node && node.parentNode && node.parentNode.data) {
            return node.parentNode.data.ObjectId;
        }

        return null;
    },

    // Нужно ли сбросить выбранные продукты
    needResetCheckboxes: function (inOutSelectionProductWindowController, previousProductTreeNodes, currentProductTreeNodes) {
        // При первом выборе нет предыдущих узлов
        if (previousProductTreeNodes.length == 0) {
            return false;
        }

        // Если предыдущие и текущие узлы одинаковые
        var previousProductTreeNodesEqualsCurrent = inOutSelectionProductWindowController.compareArrays(previousProductTreeNodes, currentProductTreeNodes);
        if (previousProductTreeNodesEqualsCurrent) {
            return false;
        }

        // Если выбранные узлы это сабренжи с одинаковым родителем
        var allNodesAreSubranges = inOutSelectionProductWindowController.allNodesAreSubranges(previousProductTreeNodes) && inOutSelectionProductWindowController.allNodesAreSubranges(currentProductTreeNodes);
        var oneParent = inOutSelectionProductWindowController.getNodeParentObjectId(previousProductTreeNodes[0]) == inOutSelectionProductWindowController.getNodeParentObjectId(currentProductTreeNodes[0]);
        if (allNodesAreSubranges && oneParent) {
            return false;
        }

        return true;
    },

    onProductGridSelectionChange: function (model, selected) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var productGrid = Ext.ComponentQuery.query('product')[0].down('grid');
        var productGridStore = productGrid.getStore();
        var productGridSelectionModel = productGrid.getSelectionModel();

        inOutSelectionProductWindowController.checkSelectAll(productGridSelectionModel, productGridStore);
        this.setInfoGridToolbar(productGrid);
    },

    onProductAfterRender: function (panel) {
        var grid = panel.down('grid');
        var store = grid.getStore();
        var proxy = store.getProxy();

        panel.addListener('beforedestroy', function () {
            proxy.extraParams.inOutProductTreeObjectIds = '';
            proxy.extraParams.needInOutFilteredProducts = false;
            proxy.extraParams.needInOutExcludeAssortmentMatrixProducts = false;
            proxy.extraParams.needInOutSelectedProducts = false;
            proxy.extraParams.inOutProductIdsForGetting = '';
        })
    },

    setInfoGridToolbar: function (productGrid) {
        productGrid = productGrid || Ext.ComponentQuery.query('product')[0].down('grid');
        var productGridSelectionModel = productGrid.getSelectionModel();

        if (productGridSelectionModel.getCheckedRows) {
            var productGridStore = productGrid.getStore();
            var checkedRows = productGridSelectionModel.getCheckedRows();

            var recordsCount = productGridStore.getTotalCount();
            var checkedRowsInGrid = [];

            var recordsCountMessage = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), recordsCount);
            var checkedCountMessage = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('checkedRecordsCount'), 0);
            var resultMessage = recordsCountMessage + ' ' + '<b>' + checkedCountMessage + '</b>';
            productGrid.query('#displayItem')[0].setText(resultMessage);

            if (recordsCount > 0) {
                var records = productGridStore.getRange(0, recordsCount, {
                    callback: function () {
                        records = productGridStore.getRange(0, recordsCount);

                        checkedRowsInGrid = records.filter(function (record) {
                            return checkedRows.some(function (checkedRow) { return record.data.Id == checkedRow.data.Id })
                        });

                        recordsCountMessage = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), records.length);
                        checkedCountMessage = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('checkedRecordsCount'), checkedRowsInGrid.length);
                        resultMessage = recordsCountMessage + ' ' + '<b>' + checkedCountMessage + '</b>';

                        productGrid.query('#displayItem')[0].setText(resultMessage);
                    }
                });
            }
        }
    },

    onSelectAllRecordsClick: function (headerCt, header) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var productGrid = header.up('directorygrid');
        var productGridStore = productGrid.getStore();
        var productGridSelectionModel = productGrid.getSelectionModel();
        var recordsCount = productGridStore.getCount();
        var checkedRowsCount = productGridSelectionModel.getCheckedRows(); 

        productGridSelectionModel.selectAllRecordsCallback = inOutSelectionProductWindowController.setInfoGridToolbar;
        productGridSelectionModel.deselectAllRecordsCallback = inOutSelectionProductWindowController.setInfoGridToolbar;

        inOutSelectionProductWindowController.checkSelectAll(productGridSelectionModel, productGridStore);
    },

    // Следит за правильным состоянием флага allSelect
    checkSelectAll: function (productGridSelectionModel, productGridStore) {
        var checkedRows = productGridSelectionModel.getCheckedRows();
        var recordsCount = productGridStore.getTotalCount();

        if (recordsCount > 0) {
            var records = productGridStore.getRange(0, recordsCount, {
                callback: function () {
                    records = productGridStore.getRange(0, recordsCount);

                    // Если все записи из грида есть в выбранных
                    if (records.every(function (record) { return checkedRows.some(function (checkedRow) { return record.data.Id == checkedRow.data.Id }) })) {
                        productGridSelectionModel.allSelected = true;
                    } else {
                        productGridSelectionModel.allSelected = false;
                    }
                }
            });
        }
    },

    // Отметить только те записи, что находятся в гриде
    removeHiddenRowsFromSelectionModel: function (selectionModel, productGrid) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var productGridStore = productGrid.getStore();
        var hasExtendedFilter = productGridStore.getExtendedFilter() && productGridStore.getExtendedFilter().filter;
        var hasTableFilter = productGridStore.filters.items.length > 0;
        var excludeAssortmentMatrixProductsButtonPressed = productGrid.up('product').down('#excludeassortmentmatrixproductsbutton').pressed;

        // Если не применены фильтры и не нажата кнопка матрицы
        if (!hasExtendedFilter && !hasTableFilter && !excludeAssortmentMatrixProductsButtonPressed && selectionModel) {
            inOutSelectionProductWindowController.setInfoGridToolbar(productGrid);
            var recordsCount = productGridStore.getTotalCount();
            if (recordsCount > 0) {
                var records = productGridStore.getRange(0, recordsCount, {
                    callback: function () {
                        records = productGridStore.getRange(0, recordsCount);
                        var checkedRows = selectionModel.getCheckedRows();

                        selectionModel.uncheckRows(checkedRows);
                        checkedRows = checkedRows.filter(function (checkedRow) {
                            return records.some(function (record) { return checkedRow.data.Id == record.data.Id; });
                        });
                        selectionModel.checkRows(checkedRows);

                        inOutSelectionProductWindowController.checkSelectAll(selectionModel, productGridStore);
                        inOutSelectionProductWindowController.setInfoGridToolbar(productGrid);
                    }
                });
            }
        }
    },

    // Возвращает строку из ID элкментов через ;
    parseRecordsToString: function (records) {
        var result = ''; 
        records.forEach(function (record) {
            result += record.data.Id + ';';
        });
        return result;
    },

    // Возвращает строку из исключенных продуктов через ;
    getRegularExcludedProductsString: function (productRecords, checkedProductRecords) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');

        var excludedRecords = productRecords.filter(function (productRecord) {
            return !checkedProductRecords.some(function (checkedProductRecord) {
                return checkedProductRecord.data.Id == productRecord.data.Id;
            });
        });

        return inOutSelectionProductWindowController.parseRecordsToString(excludedRecords);
    },

    // Получить массив продуктовых записей по массиву из Id
    checkProductsByProductIds: function (productGrid, productIdsArray, productTreeGrid, promoEditorCustom) {
        var productGridStore = productGrid.getStore();
        var productGridSelectionModel = productGrid.getSelectionModel();
        var recordsCount = productGridStore.getTotalCount();

        if (productGridSelectionModel && recordsCount > 0) {
            var records = productGridStore.getRange(0, recordsCount, {
                callback: function () {
                    records = productGridStore.getRange(0, recordsCount);
                    records = records.filter(function (record) {
                        return productIdsArray.some(function (productId) { return record.data.Id == productId; });
                    });

                    productGridSelectionModel.checkRows(records);

                    // Если промо регулярное, отметить все продукты при выборе узла.
                    // Вешается именно здесь не случайно.
                    if (!promoEditorCustom.isInOutPromo) {
                        productTreeGrid.addListener('checkchange', function (item, checked) {
                            if (checked) {
                                productGridStore.on({
                                    load: {
                                        fn: function () {
                                            var productGridSelectionModel = productGrid.getSelectionModel();
                                            productGridSelectionModel.selectAllRecords(productGrid.down('gridcolumn[cls=select-all-header]'));
                                            productGridSelectionModel.allSelected = true;
                                        },
                                        single: true
                                    },
                                });
                            }
                        });
                    }
                }
            });
        }
    }
});
