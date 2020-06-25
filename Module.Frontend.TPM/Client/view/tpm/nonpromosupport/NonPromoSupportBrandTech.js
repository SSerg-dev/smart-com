Ext.define('App.view.tpm.nonpromosupport.NonPromoSupportBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.nonpromosupportbrandtech',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BrandTech'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
        }, {
            itemId: 'addbutton',
            action: 'Post',
            glyph: 0xf415,
            text: l10n.ns('core', 'crud').value('createButtonText'),
            tooltip: l10n.ns('core', 'crud').value('createButtonText')
        }, {
            itemId: 'deletebutton',
            action: 'Delete',
            disabled: true,
            setDisabled: function (firstCondition, secondCondition) {
                if (firstCondition === true && secondCondition === true) {
                    this.disable();
                } else if (firstCondition === false && secondCondition === false) {
                    this.enable();
                }
            },
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }],
        dock: 'right'
    }],

    minHeight: 0,

    systemHeaderItems: [],
    customHeaderItems: [],

    style: {
        "border-left": "1px solid #ccd1d9",
        "margin": "5px 0 5px 0",
        "box-shadow": "0px 3px 8px rgba(0,0,0,.25)"
    },

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            autoLoad: false,
            model: 'App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTech',
            storeId: 'nonpromosupportbrandtechstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTech',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: false,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechBrandName'),
                dataIndex: 'BrandTechBrandName'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechTechnologyName'),
                dataIndex: 'BrandTechTechnologyName'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechSubName'),
                dataIndex: 'BrandTechSubName'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechBrandTech_code'),
                dataIndex: 'BrandTechBrandTech_code'
            }, {
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechBrandsegTechsub_code'),
                dataIndex: 'BrandTechBrandsegTechsub_code'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTech',
        items: []
    }]
});