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
                    beforerender: this.onProductTreeGridBeforeRender,
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

    onOkButtonClick: function (button) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var productTreeGrid = button.up('window').down('producttreegrid');
        var product = button.up('window').down('product');
        var productGrid = product.down('grid');
        var excludeAssortmentMatrixProductsButton = product.down('#excludeassortmentmatrixproductsbutton');
        var checkedRecords = productGrid.getSelectionModel().getCheckedRows();
        var promoController = App.app.getController('tpm.promo.Promo');

        var inOutProductIds = '';
        if (productTreeGrid.getChecked().length > 0 && checkedRecords.length > 0) {
            checkedRecords.forEach(function (product) {
                inOutProductIds += product.data.Id + ';';
            });

            promoEditorCustom.InOutProductIds = inOutProductIds;

            var productTreeCheckedNodes = productTreeGrid.getChecked();

            var productTreeNodes = [];
            productTreeCheckedNodes.forEach(function (node) {
                productTreeNodes.push(node);
            });
            promoEditorCustom.productTreeNodes = productTreeNodes;
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
    },

    onProductTreeGridBeforeRender: function (productTreeGrid) {
        productTreeGrid.selModel.selectionMode = 'MULTI';
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
            var productsRootNodeObjectId = productTreeGrid.getRootNode().childNodes[0].data.ObjectId;
            if (productsRootNodeObjectId == '1000000') {
                productTreeGrid.setRootNode(productTreeGrid.getRootNode().childNodes[0], false);
                productTreeGrid.getRootNode().expand();
            }

            var targetNodes = productTreeGrid.getChecked();
            me.getProductTreeController().setCheckTree(productTreeGrid.getRootNode().childNodes, false, targetNodes);
            me.controllCheckProduct(productTreeGrid, targetNodes);

            productTreeGridStoreProxy.extraParams.productTreeObjectIds = null;
            excludeassortmentmatrixproductsbutton.fireEvent('toggle', excludeassortmentmatrixproductsbutton, excludeassortmentmatrixproductsbutton.pressed);

            inOutSelectionProductWindowController.setInfoGridToolbar(Ext.ComponentQuery.query('product')[0].down('grid'));
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
        var store = item.store;
        var treegrid = store.ownerTree;
        var nodes = treegrid.getRootNode().childNodes;
        var c = this.getController('tpm.product.ProductTree');
        var inOutProductSelectionWindowProductTreeGridContainer = treegrid.up('#InOutProductSelectionWindowProductTreeGridContainer');
        var product = treegrid.up('inoutselectionproductwindow').down('product');
        var productGrid = product.down('grid');
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

    productGridBeforeRender: function (grid) {
        grid.multiSelect = true;
    },

    getProductTreeController: function () {
        return App.app.getController('tpm.product.ProductTree');
    },

    onProductEditorAfterRender: function (window) {
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
        var productGridStore = productGrid.getStore();
        var productGridStoreProxy = productGridStore.getProxy();
        var toolBar = window.down('custombigtoolbar');
        var productTreeGrid = window.down('producttreegrid');

        if (isPressed) {
            button.checkedProducts = productGrid.getSelectionModel().getCheckedRows();
        } else {
            productGrid.getSelectionModel().checkRows(button.checkedProducts);
            //button.checkedProducts = [];
        }

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
        var productGridSelectiomModel = productGrid.getSelectionModel();
        var isFirstOpening = true;
        var excludeAssortmentMatrixProductsButton = productGrid.up('product').down('#excludeassortmentmatrixproductsbutton');

        me.setInfoGridToolbar(productGrid);

        // Id выбранных галками продуктов
        var tempInOutCheckedProductIds = [];
        // Предыдущие выбранные узлы дерева
        var previousProductTreeNodes = [];
        // Текущие выбранные узлы дерева
        var currentProductTreeNodes = [];

        productGrid.getStore().addListener('prefetch', function (store, records) {
            me.setInfoGridToolbar(productGrid);
        });

        // Шаг 1: запонимаем куда-нибудть выбранные продукты
        // Шаг 2: сбрасываем галки со всех продуктов (галки стоят даже на тех продкутах, которых нет в гриде)
        // Шаг 3: из запомненных продуктов на шаге 1, отмечаем те, которые есть в гриде
        productGrid.getStore().addListener('load', function (store, records) {
            var checkedProducts = productGridSelectiomModel.getCheckedRows();
            tempInOutCheckedProductIds = [];
            checkedProducts.forEach(function (checkedProduct) {
                tempInOutCheckedProductIds.push(checkedProduct.data.Id);
            });
            inOutSelectionProductWindowController.customUncheckRows(productGridSelectiomModel, checkedProducts);

            var checkedProductTreeLeafsArray = inOutSelectionProductWindowController.getCheckedProductTreeLeafsArray(productTreeGrid.getChecked());
            var tempCurrentProductTreeNodes = [];
            checkedProductTreeLeafsArray.forEach(function (leaf) {
                tempCurrentProductTreeNodes.push(leaf);
            });
            currentProductTreeNodes = tempCurrentProductTreeNodes;

            var needResetCheckboxes = inOutSelectionProductWindowController.needResetCheckboxes(inOutSelectionProductWindowController, previousProductTreeNodes, currentProductTreeNodes);
            if (needResetCheckboxes) {
                tempInOutCheckedProductIds = [];
                inOutSelectionProductWindowController.customUncheckRows(productGridSelectiomModel, checkedProducts);
                excludeAssortmentMatrixProductsButton.checkedProducts = [];
            } else {
                var inOutProductIds = [];
                if (isFirstOpening) {
                    inOutProductIds = tempInOutCheckedProductIds.length > 0 ? tempInOutCheckedProductIds : me.getProductIdsList(promoEditorCustom.InOutProductIds || (promoModel ? promoModel.data.InOutProductIds : ''));
                } else {
                    inOutProductIds = tempInOutCheckedProductIds;
                }

                if (inOutProductIds) {
                    var productModels = [];
                    inOutProductIds.forEach(function (productId) {
                        if (productId) {
                            var searchedProductModel = productGrid.getStore().getById(productId);
                            if (searchedProductModel) {
                                productModels.push(searchedProductModel);
                            }
                        }
                    });
                    me.customCheckRows(productGridSelectiomModel, productModels);
                }

                if (productGridStore.getTotalCount() == 0) {
                    me.customUncheckRows(productGridSelectiomModel, productGrid.getSelectionModel().getCheckedRows());
                    tempInOutCheckedProductIds = [];
                }
            }

            // Если выбраны все записи
            if (productGridSelectiomModel.getCheckedRows().length == records.length) {
                productGridSelectiomModel.allSelected = true;
            }
            else {
                productGridSelectiomModel.allSelected = false;
            }

            // Предыдущие выбранные становятся текущими выбранным.
            if (currentProductTreeNodes.length > 0) {
                previousProductTreeNodes = currentProductTreeNodes;
            }

            isFirstOpening = false;
            me.setInfoGridToolbar(productGrid);
        });
    },

    customCheckRows: function (selectionModel, records) {
        records.forEach(function (record) {
            record.data.Checked = true;
        });

        selectionModel.checkRows(records);
    },

    customUncheckRows: function (selectionModel, records) {
        records.forEach(function (record) {
            record.data.Checked = false;
        });

        selectionModel.uncheckRows(records);
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
        // При первом октрытии сохраненного промо previousProductTreeNodes = null
        if (previousProductTreeNodes.length == 0) {
            return false;
        }

        var previousProductTreeNodesEqualsCurrent = inOutSelectionProductWindowController.compareArrays(previousProductTreeNodes, currentProductTreeNodes);
        var allNodesAreSubranges = inOutSelectionProductWindowController.allNodesAreSubranges(previousProductTreeNodes) && inOutSelectionProductWindowController.allNodesAreSubranges(currentProductTreeNodes);
        var oneParent = inOutSelectionProductWindowController.getNodeParentObjectId(previousProductTreeNodes[0]) == inOutSelectionProductWindowController.getNodeParentObjectId(currentProductTreeNodes[0]);

        // Если не subranges с одинаковым родителем
        if (!previousProductTreeNodesEqualsCurrent && !(allNodesAreSubranges && oneParent)) {
            return true;
        }

        return false;
    },

    onProductGridSelectionChange: function (model, selected) {
        this.setInfoGridToolbar(Ext.ComponentQuery.query('product')[0].down('grid'));
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

    setInfoGridToolbar: function (grid) {
        if (grid) {
            var recordsCount = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), grid.getStore().getTotalCount());
            var checkedCount = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('checkedRecordsCount'), grid.getSelectionModel().getCheckedRows().length);
            var msg = recordsCount + ' ' + '<b>' + checkedCount + '</b>';

            grid.query('#displayItem')[0].setText(msg);
        }
    },

    onSelectAllRecordsClick: function (headerCt, header) {
        var inOutSelectionProductWindowController = App.app.getController('tpm.inoutselectionproductwindow.InOutSelectionProductWindow');
        var grid = header.up('directorygrid');
        var store = grid.getStore();
        var selModel = grid.getSelectionModel()
        var recordsCount = store.getCount();
        var functionChecker = selModel.checkedRows.length == recordsCount ? selModel.uncheckRows : selModel.checkRows;

        for (var i = 0; i < recordsCount; i++) {
            functionChecker.call(selModel, store.getAt(i));
        }

        inOutSelectionProductWindowController.setInfoGridToolbar(grid);
    }
});
