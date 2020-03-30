Ext.define('App.controller.tpm.clientdashboard.ClientDashboard', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'clientdashboard': {
                    beforerender: this.onClientDashboardBeforeRender,
                    afterrender: this.onClientDashboardAfterRender,
                },

                'clientdashboard #refresh': {
                    click: this.onRefreshButtonClick
                },

                'accountinformation #panelButton': {
                    afterrender: this.onClientTreePanelAfterRender
                },
                'accountinformation #detailsButton': {
                    click: this.onDetailButtonClick
                }, 
            }
        });
    },
    onDetailButtonClick: function (button) { 
        var window = Ext.widget('clentdashboarddetailswindow');
        window.show(); 
        window.down('[name=POSMInClientYTD]').setValue(button.POSMInClientYTD); 
        window.down('[name=CatalogueYTD]').setValue(button.CatalogueYTD); 
        window.down('[name=XSitesYTD]').setValue(button.XSitesYTD ); 
        window.down('[name=CatalogueYEE]').setValue(button.CatalogueYEE); 
        window.down('[name=POSMInClientTiYEE]').setValue(button.POSMInClientTiYEE); 
        window.down('[name=XSitesYEE]').setValue(button.XSitesYEE); 
    },
   
    onClientDashboardBeforeRender: function (panel) {
        panel.setLoading(true);

        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard'); 
        var clientDashboardClientYearWindowChoose = Ext.widget('clientdashboardclientyearchoosewindow');
        var clientTreeField = clientDashboardController.getClientTreeField(clientDashboardClientYearWindowChoose);

        clientDashboardClientYearWindowChoose.down('#choose').addListener('click', clientDashboardController.onClientDashboardClientYearWindowChooseButtonClick);

        var clientTreeStore = clientDashboardController.getClientTreeStore();
        var clientTreeStoreProxy = clientTreeStore.getProxy();

        panel['choosenClientTreeRecord'] = new App.model.tpm.clienttree.ClientTree();
        panel['choosenYear'] = null;

        clientTreeStoreProxy.extraParams.needBaseClients = true;
        clientTreeStoreProxy.extraParams['node'] = 'root';

        // Получаем базовых клиентов для текущего пользователя.
        // Если только один базовый, то выбираем его по умолчанию.
        clientDashboardClientYearWindowChoose.setLoading(true);
        clientTreeStore.on({
            load: {
                fn: function (store, records) {
                    panel['loaderState'] = true;
                    var baseClients = [];

                    var stack = records;
                    while (stack.length > 0) {
                        var currentNode = stack.pop();
                        if (currentNode) {
                            if (currentNode.IsBaseClient) {
                                baseClients.push(currentNode);
                            }

                            var childs = [];
                            if ((currentNode.raw && currentNode.raw.children)) {
                                childs = Array.isArray(currentNode.raw.children) ? currentNode.raw.children : [currentNode.raw.children];
                            }
                            else if (currentNode.children) {
                                childs = currentNode.children;
                            }

                            childs.forEach(function (x) {
                                stack.push(x);
                            })
                        }
                    }

                    if (baseClients.length == 1) {
                        clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                            Id: baseClients[0].Id,
                            Name: baseClients[0].Name,
                            ObjectId: baseClients[0].ObjectId,
                        }));

                        clientDashboardClientYearWindowChoose.down('#choose').fireEvent('click', clientDashboardClientYearWindowChoose.down('#choose'));
                    }
                    else {
                        clientDashboardClientYearWindowChoose.show(null, function () {
                            panel.setLoading(false);
                        });
                    }

                    clientTreeStoreProxy.extraParams.needBaseClients = false;
                    delete clientTreeStoreProxy.extraParams.node;
                },
                single: true
            }
        });

        clientTreeStore.load();
    },

    onClientDashboardAfterRender: function (panel) {
        if (!panel['loaderState']) {
            panel.setLoading(true);
        }
    },

    onRefreshButtonClick: function (button) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard'); 
        var clientDashboard = Ext.ComponentQuery.query('clientdashboard')[0];

        clientDashboardController.loadStoreWithFilters(clientDashboard, clientDashboardController.fillAccountInformationCallback, clientDashboardController.fillPromoWeeksCallback,true);
    },

    onClientDashboardClientYearWindowChooseButtonClick: function (button) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard'); 
        var clientDashboard = Ext.ComponentQuery.query('clientdashboard')[0];
        var clientDashboardClientYearWindowChoose = button.up('clientdashboardclientyearchoosewindow');
        var clientTreeField =  clientDashboardClientYearWindowChoose.down('#ClientTreeField');
        var yearField = clientDashboardClientYearWindowChoose.down('#YearField');

        clientTreeField.validate();
        yearField.validate();

        var selectedClientRecord = clientTreeField.getRecord();
        var selectedYear = yearField.getValue();

        if (clientTreeField.isValid() && yearField.isValid()) {
            clientDashboard.show();
            var accountInformationButton = clientDashboard.down('#accountInformationButton');
            var promoWeeksButton = clientDashboard.down('#promoWeeksButton');
            var accountInformation = clientDashboard.down('accountinformation');

            clientDashboard['choosenClientTreeRecord'] = selectedClientRecord;
            clientDashboard['choosenYear'] = selectedYear;

            accountInformation.down('#accountInformationClientText').setText(selectedClientRecord.data.Name);
            accountInformation.down('#accountInformationYearText').setText(selectedYear);

            clientDashboardController.loadStoreWithFilters(clientDashboard, clientDashboardController.fillAccountInformationCallback, clientDashboardController.fillPromoWeeksCallback,false);
            button.up('window').close();
        }
    },

    getClientTreeStore: function () {
        var clientTreeStore = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.clienttree.ClientTree',
        });

        return clientTreeStore;
    },

    onTrigger1Click: function (picker) {
        var picker = picker.createPicker();
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var clientTreeField = clientDashboardController.getClientTreeField();

        var clientTreeStore = clientDashboardController.getClientTreeStore();
        var clientTreeStoreProxy = clientTreeStore.getProxy();

        if (clientTreeField.getRecord()) {
            clientTreeStoreProxy.extraParams.clientObjectId = clientTreeField.getRecord().data.ObjectId;
        } else {
            clientTreeStoreProxy.extraParams.clientObjectId = null;
        }

        if (picker) {
            var clientTree = picker.down(this.selectorWidget);
            var clientTreeGrid = clientTree.down('basetreegrid');
            var clientTreeGridStore = clientTreeGrid.getStore();

            clientTree.chooseMode = true;
            if (clientTreeField.getRecord()) {
                clientTree.choosenClientObjectId = clientTreeField.getRecord().data.ObjectId;
            }

            clientTreeGridStore.addListener('load', function () {
                clientDashboardController.showCheckboxesForOnlyBaseClients(clientTreeGridStore.getRootNode().childNodes, clientTreeGrid.getChecked());
            });

            clientTreeGrid.addListener('checkchange', clientDashboardController.onClientTreeCheckChange);
            clientTree.down('basetreegrid').up('window').down('#select').addListener('click', function () { clientDashboardController.onSelectClientTreeInPicker(picker, clientTreeGrid.getChecked()[0]); });

            picker.show();

            var header = clientTree.getHeader();
            var splitter = clientTree.down('#splitter_1');
            var settings = clientTree.down('#clientTreeSettingsPanel');
            var addNodeButton = clientTree.down('#addNode');
            var deleteNodeButton = clientTree.down('#deleteNode');

            header.hide();
            splitter.hide();
            settings.hide();
            addNodeButton.hide();
            deleteNodeButton.hide();
        }
    },

    getClientTreeField: function (parent) {
        var clientTreeField = null;
        if (parent) {
            clientTreeField = parent.down('#ClientTreeField');
        }
        else {
            clientTreeField = Ext.ComponentQuery.query('#ClientTreeField')[0];
        }
        return clientTreeField;
    },

    onClientTreeCheckChange: function (item, checked, eOpts) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var treegrid = item.store.ownerTree;
        var nodes = treegrid.getRootNode().childNodes;
        var clientTree = treegrid.up('clienttree');
        var editorForm = clientTree.down('editorform');
        var clientTreeController = App.app.getController('tpm.client.ClientTree');
        var clientChooseWindow = clientTree.up('window');
        var chooseButton = clientChooseWindow.down('#select');

        clientDashboardController.showCheckboxesForOnlyBaseClients(nodes);
        if (checked) {
            item.set('checked', true);
            chooseButton.enable();
        } else {
            chooseButton.disable();
        }
    },

    showCheckboxesForOnlyBaseClients: function (nodes, checkedNodes) {
        var me = this;
        if (nodes && nodes.length > 0) {
            nodes.forEach(function (node, index, array) {
                if (node.data.IsBaseClient) {
                    var nodeHtml = node.getOwnerTree().getView().getNode(node);
                    if (nodeHtml) {
                        Ext.fly(nodeHtml).addCls('hierarchy-baseclient');
                    }

                    if (checkedNodes && checkedNodes.length > 0 && checkedNodes[checkedNodes.length - 1].data.ObjectId == node.data.ObjectId) {
                        node.set('checked', true);
                    } else {
                        node.set('checked', false);
                    }
                } else {
                    node.set('checked', null);
                }

                me.showCheckboxesForOnlyBaseClients(node.childNodes, checkedNodes);
            });
        }
    },

    onSelectClientTreeInPicker: function (picker, clientTreeRecord) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var clientTreeField = clientDashboardController.getClientTreeField();

        if (clientTreeRecord) {
            clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                Id: clientTreeRecord.data.Id,
                Name: clientTreeRecord.data.Name,
                ObjectId: clientTreeRecord.data.ObjectId,
            }));
        }

        picker.close(); 
    },

    onClientTreePanelAfterRender: function (panel) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        panel.body.addListener('click', function () {
            clientDashboardController.onClientYearPanelClick(panel);
        });
    },

    onClientYearPanelClick: function (panel) {
        var clientDashboardController = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var clientDashboard = panel.up('clientdashboard');
        var clientDashboardClientYearWindowChoose = Ext.widget('clientdashboardclientyearchoosewindow');
        var clientTreeField = clientDashboardController.getClientTreeField(clientDashboardClientYearWindowChoose);
        var yearField = clientDashboardClientYearWindowChoose.down('#YearField');

        var accountInformationYearText = panel.down('#accountInformationYearText');

        var clientTreeRecord = clientDashboard['choosenClientTreeRecord'];
        if (clientTreeRecord) {
            clientTreeField.setValue(new App.model.tpm.clienttree.ClientTree({
                Id: clientTreeRecord.data.Id,
                Name: clientTreeRecord.data.Name,
                ObjectId: clientTreeRecord.data.ObjectId,
            }));
            clientDashboard['choosenClientTreeRecord'] = clientTreeRecord;
            clientDashboard['choosenYear'] = accountInformationYearText.text;
        }

        clientDashboardClientYearWindowChoose.down('#choose').addListener('click', clientDashboardController.onClientDashboardClientYearWindowChooseButtonClick);
        clientDashboardClientYearWindowChoose.show();

        yearField.setValue(accountInformationYearText.text);
    },

    fillAccountInformationCallback: function (records, clientDashboard) {
        var controller = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var accountinformation = clientDashboard.down('accountinformation');
        var ROIPanel = clientDashboard.down('#ROIChartPanel');
        var incrementalNSV = clientDashboard.down('#panelIncPromoFirst2');
        var promoNSV = clientDashboard.down('#panelIncPromoSecond2');
        var shopperTIPlanPercent = 0;
        var shopperTIYTDPercent = 0;
        var shopperTIYEEPercent = 0;
        var shopperTIPlanMln = 0;
        var shopperTIYTDMln = 0;
        var shopperTIYEEMln = 0;
        var marketingTIPlanPercent = 0;
        var marketingTIYTDPercent = 0;
        var marketingTIYEEPercent = 0;
        var marketingTIPlanMln = 0;
        var marketingTIYTDMln = 0;
        var marketingTIYEEMln = 0;
        var productionPlanPercent = 0;
        var productionYTDPercent = 0;
        var productionYEEPercent = 0;
        var productionPlanMln = 0;
        var productionYTDMln = 0;
        var productionYEEMln = 0;
        var brandingPlanPercent = 0;
        var brandingYTDPercent = 0;
        var brandingYEEPercent = 0;
        var brandingPlanMln = 0;
        var brandingYTDMln = 0;
        var brandingYEEMln = 0;
        var btlPlanPercent = 0;
        var btlYTDPercent = 0;
        var btlYEEPercent = 0;
        var btlPlanMln = 0;
        var btlYTDMln = 0;
        var btlYEEMln = 0;
        var roiPlanPercent = 0;
        var roiYTDPercent = 0;
        var roiYEEPercent = 0;
        var lsvPlanMln = 0;
        var lsvYTDMln = 0;
        var lsvYEEMln = 0;
        var incrementalNSVPlanMln = 0;
        var incrementalNSVYTDMln = 0;
        var incrementalNSVYEEMln = 0;
        var promoNSVPlanMln = 0;
        var promoNSVYTDMln = 0;
        var promoNSVYEEMln = 0;
        var pOSMInClientYTD = 0;
        var catalogueYTD = 0;
        var xSitesYTD = 0;
        var catalogueYEE = 0;
        var pOSMInClientTiYEE = 0;
        var xSitesYEE = 0;

        var ActualPromoLSV = 0;
        var PlanAndActualPromoLSV = 0;
        var TotalPromoIncrementalEarnings = 0;
        var ActualPromoCost = 0;
        var TotalPromoCost = 0;
        var ActualPromoIncrementalEarnings = 0;

        records.forEach(function (record) {
            shopperTIPlanPercent += record.data.ShopperTiPlanPercent;
            shopperTIYTDMln += record.data.ShopperTiYTD;
            shopperTIYEEMln += record.data.ShopperTiYEE;

            marketingTIPlanPercent += record.data.MarketingTiPlanPercent;
            marketingTIYTDMln += record.data.MarketingTiYTD;
            marketingTIYEEMln += record.data.MarketingTiYEE;

            productionPlanMln += record.data.ProductionPlan;
            productionYTDMln += record.data.ProductionYTD;
            productionYEEMln += record.data.ProductionYEE;

            brandingPlanMln += record.data.BrandingPlan;
            brandingYTDMln += record.data.BrandingYTD;
            brandingYEEMln += record.data.BrandingYEE;

            btlPlanMln += record.data.BTLPlan;
            btlYTDMln += record.data.BTLYTD;
            btlYEEMln += record.data.BTLYEE;

            roiPlanPercent += record.data.ROIPlanPercent;

            lsvPlanMln += record.data.LSVPlan;
            lsvYTDMln += record.data.LSVYTD;
            lsvYEEMln += record.data.LSVYEE;

            incrementalNSVPlanMln += record.data.IncrementalNSVPlan;
            incrementalNSVYTDMln += record.data.IncrementalNSVYTD;
            incrementalNSVYEEMln += record.data.IncrementalNSVYEE;

            promoNSVPlanMln += record.data.PromoNSVPlan;
            promoNSVYTDMln += record.data.PromoNSVYTD;
            promoNSVYEEMln += record.data.PromoNSVYEE;

            pOSMInClientYTD += record.data.POSMInClientYTD;
            catalogueYTD += record.data.CatalogueYTD;
            xSitesYTD += record.data.XSitesYTD;
            catalogueYEE += record.data.CatalogueYEE;
            pOSMInClientTiYEE += record.data.POSMInClientTiYEE;
            xSitesYEE += record.data.XSitesYEE;

            ActualPromoLSV += record.data.ActualPromoLSV;
            PlanAndActualPromoLSV += record.data.PlanPromoLSV;
            ActualPromoIncrementalEarnings += record.data.ActualPromoIncrementalEarnings;
            TotalPromoIncrementalEarnings += record.data.TotalPromoIncrementalEarnings;
            ActualPromoCost += record.data.ActualPromoCost;
            TotalPromoCost += record.data.TotalPromoCost;
        });
        PlanAndActualPromoLSV = PlanAndActualPromoLSV + ActualPromoLSV;

        shopperTIPlanPercent = shopperTIPlanPercent / records.length;
        marketingTIPlanPercent = marketingTIPlanPercent / records.length;
        roiPlanPercent = roiPlanPercent / records.length;

        shopperTIPlanMln = shopperTIPlanPercent * lsvPlanMln / 100;
        marketingTIPlanMln = marketingTIPlanPercent * lsvPlanMln / 100;

        productionPlanPercent = (lsvPlanMln != 0 ? (productionPlanMln / lsvPlanMln * 100) : 0);
        brandingPlanPercent = (lsvPlanMln != 0 ? (brandingPlanMln / lsvPlanMln * 100) : 0);
        btlPlanPercent = (lsvPlanMln != 0 ? (btlPlanMln / lsvPlanMln * 100) : 0);

        shopperTIYTDPercent = (ActualPromoLSV != 0 ? (shopperTIYTDMln / ActualPromoLSV * 100) : 0);
        marketingTIYTDPercent = (ActualPromoLSV != 0 ? (marketingTIYTDMln / ActualPromoLSV * 100) : 0);
        productionYTDPercent = (ActualPromoLSV != 0 ? (productionYTDMln / ActualPromoLSV * 100) : 0);
        brandingYTDPercent = (ActualPromoLSV != 0 ? (brandingYTDMln / ActualPromoLSV * 100) : 0);
        btlYTDPercent = (ActualPromoLSV != 0 ? (btlYTDMln / ActualPromoLSV * 100) : 0);
        roiYTDPercent = (ActualPromoCost != 0 ? (((ActualPromoIncrementalEarnings / ActualPromoCost) + 1) * 100) : 0);

        shopperTIYEEPercent = (PlanAndActualPromoLSV != 0 ? (shopperTIYEEMln / PlanAndActualPromoLSV * 100) : 0);
        marketingTIYEEPercent = (PlanAndActualPromoLSV != 0 ? (marketingTIYEEMln / PlanAndActualPromoLSV * 100) : 0);
        productionYEEPercent = (PlanAndActualPromoLSV != 0 ? (productionYEEMln / PlanAndActualPromoLSV * 100) : 0);
        brandingYEEPercent = (PlanAndActualPromoLSV != 0 ? (brandingYEEMln / PlanAndActualPromoLSV * 100) : 0);
        btlYEEPercent = (PlanAndActualPromoLSV != 0 ? (btlYEEMln / PlanAndActualPromoLSV * 100) : 0);
        roiYEEPercent = (TotalPromoCost != 0 ? (((TotalPromoIncrementalEarnings / TotalPromoCost) + 1) * 100) : 0);


        accountinformation.down('#shopperTIPlanPercent').originalValue = shopperTIPlanPercent.toFixed(2);
        accountinformation.down('#shopperTIYTDPercent').originalValue = shopperTIYTDPercent.toFixed(2);
        accountinformation.down('#shopperTIYEEPercent').originalValue = shopperTIYEEPercent.toFixed(2);
        accountinformation.down('#shopperTIPlanMln').originalValue = shopperTIPlanMln.toFixed(2);
        accountinformation.down('#shopperTIYTDMln').originalValue = shopperTIYTDMln.toFixed(2);
        accountinformation.down('#shopperTIYEEMln').originalValue = shopperTIYEEMln.toFixed(2);

        accountinformation.down('#marketingTIPlanPercent').originalValue = marketingTIPlanPercent.toFixed(2);
        accountinformation.down('#marketingTIYTDPercent').originalValue = marketingTIYTDPercent.toFixed(2);
        accountinformation.down('#marketingTIYEEPercent').originalValue = marketingTIYEEPercent.toFixed(2);
        accountinformation.down('#marketingTIPlanMln').originalValue = marketingTIPlanMln.toFixed(2);
        accountinformation.down('#marketingTIYTDMln').originalValue = marketingTIYTDMln.toFixed(2);
        accountinformation.down('#marketingTIYEEMln').originalValue = marketingTIYEEMln.toFixed(2);

        accountinformation.down('#productionPlanPercent').originalValue = productionPlanPercent.toFixed(2);
        accountinformation.down('#productionYTDPercent').originalValue = productionYTDPercent.toFixed(2);
        accountinformation.down('#productionYEEPercent').originalValue = productionYEEPercent.toFixed(2);
        accountinformation.down('#productionPlanMln').originalValue = productionPlanMln.toFixed(2);
        accountinformation.down('#productionYTDMln').originalValue = productionYTDMln.toFixed(2);
        accountinformation.down('#productionYEEMln').originalValue = productionYEEMln.toFixed(2);

        accountinformation.down('#brandingPlanPercent').originalValue = brandingPlanPercent.toFixed(2);
        accountinformation.down('#brandingYTDPercent').originalValue = brandingYTDPercent.toFixed(2);
        accountinformation.down('#brandingYEEPercent').originalValue = brandingYEEPercent.toFixed(2);
        accountinformation.down('#brandingPlanMln').originalValue = brandingPlanMln.toFixed(2);
        accountinformation.down('#brandingYTDMln').originalValue = brandingYTDMln.toFixed(2);
        accountinformation.down('#brandingYEEMln').originalValue = brandingYEEMln.toFixed(2);

        accountinformation.down('#btlPlanPercent').originalValue = btlPlanPercent.toFixed(2);
        accountinformation.down('#btlYTDPercent').originalValue = btlYTDPercent.toFixed(2);
        accountinformation.down('#btlYEEPercent').originalValue = btlYEEPercent.toFixed(2);
        accountinformation.down('#btlPlanMln').originalValue = btlPlanMln.toFixed(2);
        accountinformation.down('#btlYTDMln').originalValue = btlYTDMln.toFixed(2);
        accountinformation.down('#btlYEEMln').originalValue = btlYEEMln.toFixed(2);

        accountinformation.down('#lsvPlanMln').originalValue = lsvPlanMln.toFixed(2);
        accountinformation.down('#lsvYTDMln').originalValue = lsvYTDMln.toFixed(2);
        accountinformation.down('#lsvYEEMln').originalValue = lsvYEEMln.toFixed(2);
       
        accountinformation.down('#detailsButton').POSMInClientYTD = pOSMInClientYTD;
        accountinformation.down('#detailsButton').CatalogueYTD = catalogueYTD;
        accountinformation.down('#detailsButton').XSitesYTD = xSitesYTD;
        accountinformation.down('#detailsButton').CatalogueYEE = catalogueYEE;
        accountinformation.down('#detailsButton').POSMInClientTiYEE = pOSMInClientTiYEE;
        accountinformation.down('#detailsButton').XSitesYEE = xSitesYEE; 

        controller.setValueFieldColor(accountinformation.down('#shopperTIYTDPercent'), shopperTIPlanPercent, shopperTIYTDPercent, accountinformation.down('#shopperTIYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#shopperTIYEEPercent'), shopperTIPlanPercent, shopperTIYTDPercent, accountinformation.down('#shopperTIYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#shopperTIYTDMln'), shopperTIPlanMln, shopperTIYTDMln, accountinformation.down('#shopperTIYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#shopperTIYEEMln'), shopperTIPlanMln, shopperTIYEEMln, accountinformation.down('#shopperTIYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#marketingTIYTDPercent'), marketingTIPlanPercent, marketingTIYTDPercent, accountinformation.down('#marketingTIYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#marketingTIYEEPercent'), marketingTIPlanPercent, marketingTIYEEPercent, accountinformation.down('#marketingTIYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#marketingTIYTDMln'), marketingTIPlanMln, marketingTIYTDMln, accountinformation.down('#marketingTIYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#marketingTIYEEMln'), marketingTIPlanMln, marketingTIYEEMln, accountinformation.down('#marketingTIYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#productionYTDPercent'), productionPlanPercent, productionYTDPercent, accountinformation.down('#productionYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#productionYEEPercent'), productionPlanPercent, productionYEEPercent, accountinformation.down('#productionYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#productionYTDMln'), productionPlanMln, productionYTDMln, accountinformation.down('#productionYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#productionYEEMln'), productionPlanMln, productionYEEMln, accountinformation.down('#productionYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#brandingYTDPercent'), brandingPlanPercent, brandingYTDPercent, accountinformation.down('#brandingYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#brandingYEEPercent'), brandingPlanPercent, brandingYEEPercent, accountinformation.down('#brandingYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#brandingYTDMln'), brandingPlanMln, brandingYTDMln, accountinformation.down('#brandingYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#brandingYEEMln'), brandingPlanMln, brandingYEEMln, accountinformation.down('#brandingYEEMlnArrow'));

        controller.setValueFieldColor(accountinformation.down('#btlYTDPercent'), btlPlanPercent, btlYTDPercent, accountinformation.down('#btlYTDPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#btlYEEPercent'), btlPlanPercent, btlYEEPercent, accountinformation.down('#btlYEEPercentArrow'));
        controller.setValueFieldColor(accountinformation.down('#btlYTDMln'), btlPlanMln, btlYTDMln, accountinformation.down('#btlYTDMlnArrow'));
        controller.setValueFieldColor(accountinformation.down('#btlYEEMln'), btlPlanMln, btlYEEMln, accountinformation.down('#btlYEEMlnArrow'));

        controller.setupValuesAndTips()

        NSVChartFields = ['name', 'value'];
        var incPlan = (incrementalNSVPlanMln / 1000000).toFixed(1),
            incYTD = (incrementalNSVYTDMln / 1000000).toFixed(1),
            incYEE = (incrementalNSVYEEMln / 1000000).toFixed(1);

        var originalData = {
            Plan: incrementalNSVPlanMln,
            YTD: incrementalNSVYTDMln,
            YEE: incrementalNSVYEEMln
        }
        var incChartData = [
            { name: 'Plan', value: incPlan || 0 },
            { name: 'YTD', value: incYTD || 0 },
            { name: 'YEE', value: incYEE || 0 }
        ]
        if (incrementalNSV.items.length > 1) { // Больше 1, потому что там всегда есть 1 элемент – label
            var NSVChart = incrementalNSV.down('nsvchart');
            if (NSVChart) {
                NSVChart.destroy();
            }
            NSVChart = controller.createNSVChart(incrementalNSV, incChartData, NSVChartFields, originalData);
        } else {
            var NSVChart = controller.createNSVChart(incrementalNSV, incChartData, NSVChartFields, originalData);
        }

        var promoNSVPlan = (promoNSVPlanMln / 1000000).toFixed(1),
            promoNSVYTD = (promoNSVYTDMln / 1000000).toFixed(1),
            promoNSVYEE = (promoNSVYEEMln / 1000000).toFixed(1);

        originalData = {
            Plan: promoNSVPlanMln,
            YTD: promoNSVYTDMln,
            YEE: promoNSVYEEMln
        }
        var promoNSVChartData = [
            { name: 'Plan', value: promoNSVPlan || 0 },
            { name: 'YTD', value: promoNSVYTD || 0 },
            { name: 'YEE', value: promoNSVYEE || 0 }
        ]
        if (promoNSV.items.length > 1) {
            var promoNSVChart = promoNSV.down('nsvchart');
            if (promoNSVChart) {
                promoNSVChart.destroy();
            }
            controller.createNSVChart(promoNSV, promoNSVChartData, NSVChartFields, originalData);
        } else {
            controller.createNSVChart(promoNSV, promoNSVChartData, NSVChartFields, originalData);
        }

        var incrementalMlnLabel = accountinformation.down('#incrementalMlnLabel');
        var fixRaito = NSVChart.curWidth - NSVChart.series.items[0].bbox.width;
        incrementalMlnLabel.setWidth(incrementalMlnLabel.width + fixRaito);

        // ROI Chart
        var ROIChart = ROIPanel.down('roichart');
        if (ROIChart) {
            ROIChart.destroy();
        }
        var planROI = roiPlanPercent || 0,
            YTDROI = roiYTDPercent || 0,
            YEEROI = roiYEEPercent || 0;
        planROI = Ext.util.Format.round(planROI, 1);
        YTDROI = Ext.util.Format.round(YTDROI, 1);
        YEEROI = Ext.util.Format.round(YEEROI, 1);
        var maximum = Math.max(planROI, YTDROI, YEEROI) == 0 ? 100 : Math.max(planROI, YTDROI, YEEROI);
        maximum = maximum == 0 ? 100 : Math.ceil(maximum / 100) * 100;
        var ROIChartData = [
            { name: 'Plan', value: planROI },
            { name: 'YTD', value: YTDROI },
            { name: 'YEE', value: YEEROI },
        ];

        ROIPanel.add({
            xtype: 'roichart',
            store: Ext.create('Ext.data.Store', {
                storeId: 'roichartstore',
                fields: ['name', 'value'],
                data: ROIChartData
            }),
            width: '100%',
            height: '100%',
            maximum: maximum,
            showNegative: false,
        });
    },

    fillPromoWeeksCallback: function (records, clientDashboard) {
        var promoWeeks = clientDashboard.query('promoweeks')[0];
        promoWeeks.removePromoWeeksPanels();
        var promoWeeksPanels = [];
        records.forEach(function (record) {
            var promoWeeksPanel = Ext.widget('promoweekspanel', {
                logoFileName: record.data.LogoFileName,
                brandTechName: record.data.BrandTechName,
                promoWeeks: record.data.PromoWeeks,
                vodYTD: (record.data.VodYTD * 100).toFixed(2),
                vodYEE: (record.data.VodYEE * 100).toFixed(2)
            });
            promoWeeksPanels.push(promoWeeksPanel);
        });
        promoWeeks.addPromoWeeksPanels(promoWeeksPanels);
    },

    loadStoreWithFilters: function (clientDashboard, fillAccountInformationCallback, fillPromoWeeksCallback,refresh) {
        var clientKPIDataStore = Ext.create('App.store.core.DirectoryStore', {
            model: 'App.model.tpm.clientkpidata.ClientKPIData',
            autoLoad: false,
            sorters: [{
                property: 'BrandTechName',
                direction: 'ASC'
            }]
        })

        if (clientDashboard) {
            var filtersIds = ['ClientDashboardClientTreeObjectId', 'ClientDashboardYear'];

            var filters = [{
                property: 'ObjectId',
                operation: 'Equals',
                value: clientDashboard['choosenClientTreeRecord'].data.ObjectId
            }, {
                property: 'Year',
                operation: 'Equals',
                value: clientDashboard['choosenYear']
            }];

            clientKPIDataStore.setSeveralFixedFilters(filtersIds, filters, false);

            clientDashboard.setLoading(true);  
            clientKPIDataStore.on({
                load: {
                    fn: function (store, records) {
                        if (!refresh) {
                            if (fillAccountInformationCallback) {
                                fillAccountInformationCallback(records, clientDashboard);
                            }
                            if (fillPromoWeeksCallback) {
                                fillPromoWeeksCallback(records, clientDashboard);
                            }
                            clientDashboard.setLoading(false);
                        } else {
                            if (fillAccountInformationCallback && clientDashboard.down('#accountInformationButton').active) {
                                fillAccountInformationCallback(records, clientDashboard);
                            }
                            if (fillPromoWeeksCallback && clientDashboard.down('#promoWeeksButton').active) {
                                fillPromoWeeksCallback(records, clientDashboard);
                                
                            }
                            clientDashboard.setLoading(false);
                        }
                    },
                    single: true
                }
            });

            clientKPIDataStore.load();
        }
    },

    onAccountInformationButtonClick: function (button) {
        var clientDashboardFirstChildContainer = button.up('#clientDashboardFirstChildContainer');
        var promoWeeksButton = clientDashboardFirstChildContainer.down('#promoWeeksButton');
        var accountInformation = clientDashboardFirstChildContainer.down('#selectedContainer').down('accountinformation');
        var promoWeeks = clientDashboardFirstChildContainer.down('#selectedContainer').down('promoweeks');

        button.addCls('selected');
        button.addCls('client-dashboard-toolbar-button-selected');
        button.active = true;

        promoWeeksButton.removeCls('selected');
        promoWeeksButton.removeCls('client-dashboard-toolbar-button-selected');
        promoWeeksButton.active = false;

        promoWeeks.hide();
        accountInformation.show();
    },

    onPromoWeeksButtonClick: function (button) {
        var clientDashboardFirstChildContainer = button.up('#clientDashboardFirstChildContainer');
        var accountInformationButton = clientDashboardFirstChildContainer.down('#accountInformationButton');
        var accountInformation = clientDashboardFirstChildContainer.down('#selectedContainer').down('accountinformation');
        var promoWeeks = clientDashboardFirstChildContainer.down('#selectedContainer').down('promoweeks');

        button.addCls('selected');
        button.addCls('client-dashboard-toolbar-button-selected');
        button.active = true;

        accountInformationButton.removeCls('selected');
        accountInformationButton.removeCls('client-dashboard-toolbar-button-selected');
        accountInformationButton.active = false;

        accountInformation.hide();
        promoWeeks.show();
    },

    setValueFieldColor: function (valueField, planValue, value, arrowField) {
        if (planValue >= value) {
            valueField.removeCls('client-dashboard-account-panel-red-values');
            valueField.removeCls('client-dashboard-account-panel-blue-values');
            valueField.addCls('client-dashboard-account-panel-green-values');
            arrowField.removeCls('client-dashboard-account-panel-red-values');
            arrowField.removeCls('client-dashboard-account-panel-blue-values');
            arrowField.addCls('client-dashboard-account-panel-green-values');
            arrowField.setText('\u25B2');
        } else  {
            valueField.removeCls('client-dashboard-account-panel-green-values');
            valueField.removeCls('client-dashboard-account-panel-blue-values');
            valueField.addCls('client-dashboard-account-panel-red-values');
            arrowField.removeCls('client-dashboard-account-panel-green-values');
            arrowField.removeCls('client-dashboard-account-panel-blue-values');
            arrowField.addCls('client-dashboard-account-panel-red-values');
            arrowField.setText('\u25BC');            
        }
    },

    createNSVChart: function (container, data, fields, originalData) {
        var controller = App.app.getController('tpm.clientdashboard.ClientDashboard');
        var NSVChart = container.add({
            width: '64%',
            height: '95%',
            xtype: 'nsvchart',
            originalData: originalData,
            store: Ext.create('Ext.data.Store', {
                storeId: 'incrementalNSVstore',
                fields: fields,
                data: data,
            })
        });
        var minValue = Math.min(data[0].value, data[1].value, data[2].value);
        NSVChart.axes.items[0].maximum = Math.max(data[0].value, data[1].value, data[2].value);
        NSVChart.axes.items[0].minimum = minValue > 0 ? 0 : minValue;
        NSVChart.redraw();
        
        controller.setNSVChartZeroLine(NSVChart.axes.items[0]);
        controller.setNSVChartSeriesLabels(NSVChart);
        NSVChart.created = true;
        return NSVChart;
    },

    setNSVChartSeriesLabels: function (NSVChart) {
        // Удаляем старые, если есть
        if (NSVChart.series.seriesLabels) {
            NSVChart.series.seriesLabels.forEach(function (label) {
                NSVChart.surface.remove(label);
            });
        }
        
        var seriesLabels = [];
        NSVChart.series.items[0].items.forEach(function (item, index) {
            var chartAxes = NSVChart.axes.items[0],
                zeroY = chartAxes.chart.series.items[0].bounds.zero;
            
            var textValue = item.value[1];
            var textSprite = NSVChart.surface.add({
                type: 'text',
                fill: '#ffffff',
                text: textValue,
                x: item.attr.x + item.attr.width / 2,
                y: textValue < 0 ? zeroY + 15 : zeroY - 15,
                font: 'Bold 18px Arial',
            }).show(true);

            if ((item.attr.height < textSprite.getBBox().height + 5) || item.attr.height === 0) {
                NSVChart.surface.remove(textSprite);
            } else {
                textSprite.setAttributes({
                    translate: {
                        x: -textSprite.getBBox().width / 2,
                    }
                }, true);
                seriesLabels.push(textSprite);
            }
        });

        NSVChart.series.seriesLabels = seriesLabels;
    },

    setNSVChartZeroLine: function (chartAxes) {
        if (chartAxes.zeroLine) {
            chartAxes.chart.surface.remove(chartAxes.zeroLine);
        }

        var width = chartAxes.width,
            index = Ext.Array.indexOf(chartAxes.labels, 0) == -1 ? 0 : Ext.Array.indexOf(chartAxes.labels, 0),
            point = chartAxes.inflections[index],
            zeroY = chartAxes.chart.series.items[0].bounds.zero;
            path = ["M", point[0], zeroY, "l", width, 0],
        chartAxes.zeroLine = chartAxes.chart.surface.add({
            type: 'path',
            path: path,
            zIndex: 100,
            'stroke-width': '1px',
            stroke: '#d1d1ee'
        }).show(true);
    },

    setupValuesAndTips: function () {
        var elementsWithTips = Ext.ComponentQuery.query('*[valueField*=]');

        elementsWithTips.forEach(function (el) {
            var me = el;
            if (me.percent) {
                me.setText(me.originalValue == 0 ? 0 + "%" : me.originalValue > 10 ? parseFloat(me.originalValue).toFixed(0) + "%" : parseFloat(me.originalValue).toFixed(1) + "%");
            } else {
                me.setText((me.originalValue / 1000000) == 0 ? 0 : (parseFloat(me.originalValue / 1000000)).toFixed(1));
            }
            var value = me.originalValue == "" ? 0 : me.originalValue;
            if (me.fieldLabelTip) {
                me.fieldLabelTip.destroy();
            } 

            me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                target: me.el,
                preventLoseFocus: true,
                trackMouse: true,
                html: me.percent ? value + '%' : value,
                style: {
                    'background-color': 'rgba(63, 104, 149, 0.5)',
                    'border': 'none',
                },
                bodyStyle: 'color:#FFFFFF;'
            });
        })
    },
});