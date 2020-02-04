Ext.define('App.controller.tpm.nonpromosupport.NonPromoSupportBrandTechChoose', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'nonpromosupportbrandtechchoose #applyBrandtech': {
                    click: this.onApplyButtonClick
                },
                'nonpromosupportbrandtechchoose[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'nonpromosupportbrandtechchoose directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange,
                    selectionchange: this.onSelectionChange
                },

                'nonpromosupportbrandtechchoose #close': {
                    click: this.onCloseButtonClick
                },
                'nonpromosupportbrandtechchoose gridcolumn[cls=select-all-header]': {
                    headerclick: this.onSelectAllRecordsClick
                },
            }
        });
    },

    onGridAfterrender: function (grid) {
        var nonPromoSupportBrandTechChooseController = App.app.getController('tpm.nonpromosupport.NonPromoSupportBrandTechChoose');
        var gridStore = grid.getStore();

        gridStore.on({
            scope: this,
            load: function () {
                var applyButton = grid.up('nonpromosupportbrandtechchoose').down('#applyBrandtech');
                var selectionModel = grid.getSelectionModel();
                var checkedRows = selectionModel.getCheckedRows();

                this.updateApplyButtonState(applyButton, checkedRows);
            },
            grid: grid,
            single: true
        });

        this.callParent(arguments);
    },

    onApplyButtonClick: function (button) {
        var window = button.up('nonpromosupportbrandtechchoose'),
            grid = window.down('grid'),
            checkedRows = grid.getSelectionModel().getCheckedRows();

        if (window.associatedGrid) {
            var brandTeches = [];

            window.setLoading(l10n.ns('core').value('savingText'));

            if (checkedRows.length > 0) {
                checkedRows.forEach(function (record) {
                    brandTeches.push(record.data.Id);
                });
            }

            $.ajax({
                type: "POST",
                cache: false,
                url: "/odata/NonPromoSupportBrandTeches/ModifyNonPromoSupportBrandTechList?nonPromoSupportId=" + window.selectedNonPromoSupportRecordId,
                data: JSON.stringify(brandTeches),
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (response) {
                    var result = Ext.JSON.decode(response.value);
                    if (!result.success) {
                        App.Notify.pushError(result.message);
                    }

                    window.setLoading(false);
                    window.close();

                    var detailStore = Ext.ComponentQuery.query('nonpromosupportbrandtechdetail')[0].down('grid').getStore();
                    if (detailStore) {
                        detailStore.load();
                    }
                }
            });
        } else {
            var nonPromoSupportBrandTechStore = window.nonPromoSupportBrandTechGrid.getStore();
            var nonPromoSupportBrandTechProxy = nonPromoSupportBrandTechStore.getProxy();
            nonPromoSupportBrandTechProxy.data = [];

            checkedRows.forEach(function (item) {
                var model = {
                    BrandTechId: item.data.Id,
                    BrandTech: item.raw
                };

                var nonPromoSupportBrandTechProxyData = nonPromoSupportBrandTechProxy.getReader().readRecords(model).records[0];
                nonPromoSupportBrandTechProxy.data.push(nonPromoSupportBrandTechProxyData);
            });

            nonPromoSupportBrandTechStore.load();
            window.close();
        }
    },

    onSelectionChange: function (item) {
        var nonPromoSupportBrandTechChooseController = App.app.getController('tpm.nonpromosupport.NonPromoSupportBrandTechChoose');
        var grid = item.view.up('grid');
        var applyButton = grid.up('nonpromosupportbrandtechchoose').down('#applyBrandtech');
        var selectionModel = grid.getSelectionModel();
        var checkedRows = selectionModel.getCheckedRows();

        this.updateApplyButtonState(applyButton, checkedRows);
    },

    onSelectAllRecordsClick: function (headerCt, header) {
        var grid = header.up('directorygrid'),
            selModel = grid.getSelectionModel();

        selModel.selectAllRecordsCallback = this.selectAllButtonCallback;
        selModel.deselectAllRecordsCallback = this.selectAllButtonCallback;
    },

    selectAllButtonCallback: function () {
        var nonPromoSupportBrandTechChooseController = App.app.getController('tpm.nonpromosupport.NonPromoSupportBrandTechChoose');
        var widget = Ext.ComponentQuery.query('nonpromosupportbrandtechchoose')[0],
            grid = widget.down('brandtech grid'),
            selectionModel = grid.getSelectionModel(),
            checkedRows = selectionModel.checkedRows,
            applyButton = widget.down('#applyBrandtech');

        nonPromoSupportBrandTechChooseController.updateApplyButtonState(applyButton, checkedRows);
    },

    updateApplyButtonState: function (applyButton, checkedRows) {
        if (checkedRows.length > 0) {
            applyButton.setDisabled(false);
        } else {
            applyButton.setDisabled(true);
        }
    }
});
