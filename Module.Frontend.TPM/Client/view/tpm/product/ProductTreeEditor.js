Ext.define('App.view.tpm.product.ProductTreeEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.producttreeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    listeners: {
        afterrender: function (window) {
            window.down('searchcombobox[name=BrandId]').hide();

            var brandTechWin = window.down('searchcombobox[name = TechnologyId]');
            brandTechWin.hide();
            var brandTechStore = brandTechWin.getStore();
            var currentNode = Ext.ComponentQuery.query('producttreegrid')[0].getSelectionModel().getSelection()[0];
            var brandId = currentNode.data.BrandId;

            if (currentNode.data.Type == 'Technology') {
                window.down('textfield[name=Description_ru]').addCls('readOnlyField');
                window.down('textfield[name=Description_ru]').setReadOnly(true);
            }

            if (window.down('searchcombobox[name = Type]').value) {
                if (currentNode.parentNode) {
                    if (currentNode.parentNode.data.root) {
                        window.down('numberfield[name = NodePriority]').setDisabled(false);
                        window.down('textfield[name = Description_ru]').hide();
                    } else {
                        window.down('numberfield[name = NodePriority]').hide();
                    }
                }
            } else {
                if (currentNode.data.root) {
                    window.down('numberfield[name = NodePriority]').setDisabled(false);
                    window.down('textfield[name = Description_ru]').hide();
                } else {
                    window.down('numberfield[name = NodePriority]').hide();
                }
            }
            while (!brandId && currentNode.parentNode) {
                brandId = currentNode.data.BrandId;
                currentNode = currentNode.parentNode;
            }


            if (brandId) {
                brandTechStore.setFixedFilter('BrandId', {
                    property: 'BrandId',
                    operation: 'Equals',
                    value: brandId
                });
            }

        },
    },

    items: [{
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Name'),
            name: 'Type',
            selectorWidget: 'nodetype',
            valueField: 'Name',
            displayField: 'Name',
            entityType: 'NodeType',
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.nodetype.NodeType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.nodetype.NodeType',
                        modelId: 'efselectionmodel'
                    }]
                },
                fixedFilters: {
                    'typefilter': {
                        property: 'Type',
                        operation: 'Equals',
                        value: 'PRODUCT'
                    }
                }
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    if (newValue === 'Brand') {
                        field.up('editorform').down('searchcombobox[name=BrandId]').show();
                        field.up('editorform').down('searchcombobox[name=TechnologyId]').hide();
                        field.up('editorform').down('textfield[name=Name]').hide();
                        field.up('editorform').down('textfield[name=Description_ru]').hide();
                        field.up('editorform').down('textfield[name=Abbreviation]').setDisabled(false);
                    } else if (newValue === 'Technology') {
                        field.up('editorform').down('searchcombobox[name=BrandId]').hide();
                        field.up('editorform').down('searchcombobox[name=TechnologyId]').show();
                        field.up('editorform').down('textfield[name=Name]').hide();
                        field.up('editorform').down('textfield[name=Description_ru]').show();
                        field.up('editorform').down('textfield[name=Description_ru]').addCls('readOnlyField');
                        field.up('editorform').down('textfield[name=Description_ru]').setReadOnly(true);
                        field.up('editorform').down('textfield[name=Abbreviation]').setDisabled(false);
                    } else {
                        field.up('editorform').down('searchcombobox[name=BrandId]').hide();
                        field.up('editorform').down('searchcombobox[name=TechnologyId]').hide();
                        field.up('editorform').down('textfield[name=Name]').show();
                        field.up('editorform').down('textfield[name=Description_ru]').show();
                        field.up('editorform').down('textfield[name=Description_ru]').removeCls('readOnlyField');
                        field.up('editorform').down('textfield[name=Description_ru]').setReadOnly(false);
                        field.up('editorform').down('textfield[name=Abbreviation]').setDisabled(true);
                        field.up('editorform').down('textfield[name=NodePriority]').setDisabled(true);
                    }
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Type'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name'),
            name: 'BrandId',
            selectorWidget: 'brand',
            allowBlank: true,
            allowOnlyWhitespace: true,
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'Brand',
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.brand.Brand',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brand.Brand',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'Name'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name'),
            name: 'TechnologyId',
            selectorWidget: 'technology',
            valueField: 'TechnologyId',
            displayField: 'TechSubName',
            entityType: 'Technology',
            allowBlank: true,
            allowOnlyWhitespace: true,
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.brandtech.BrandTech',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'TechSubName',
                to: 'Name'
            }],
            onTrigger2Click: function () {
                // из-за того, что в этом элементе стор - BrandTech, а по клику на лупу должен открываться грид Technology, приходится переопределить это действие(нажатие на лупу)
                var window,
                    me = this,
                    technologyStore = Ext.create('App.store.core.SimpleStore', {
                        model: 'App.model.tpm.technology.Technology',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.technology.Technology',
                                modelId: 'efselectionmodel'
                            }]
                        }
                    }
                    );

                var selectorWidgetConfig = this.selectorWidgetConfig || {};
                selectorWidgetConfig.xtype = this.selectorWidget;

                window = this.window = Ext.widget('selectorwindow', {
                    title: l10n.ns('core').value('selectorWindowTitle'),
                    items: [selectorWidgetConfig]
                });

                if (!this.defaultStoreState) {
                    this.defaultStoreState = technologyStore.getState() || {};
                    Ext.applyIf(this.defaultStoreState, {
                        sorters: [],
                        filters: []
                    });
                }

                this.getGrid().bindStore(technologyStore);

                // в searchfield должна попасть запись BrandTech, но в гриде Technology выбирается запись Technology, поэтому выбираем нужную запись из стора BrandTech по TechnologyId
                window.down('#select').on('click', function (button) {
                    var selModel = window.down(this.selectorWidget).down('grid').getSelectionModel(),
                        technoligyRecord = selModel.hasSelection() ? selModel.getSelection()[0] : null,
                        techNameNumber = me.store.find('TechnologyId', technoligyRecord.data.Id);

                    if (techNameNumber !== -1) {
                        var record = me.store.getAt(techNameNumber);
                        me.setValue(record);
                        me.fireEvent('select', me, record);
                    }

                    window.close();
                }, this);

                window.down('#cancel').on('click', this.onCancelButtonClick, this);
                window.on('close', this.onWindowClose, this);
                this.getGrid().on('selectionchange', this.onSelectionChange, this);
                this.getGrid().getStore().on({
                    single: true,
                    scope: this,
                    load: this.onFirstLoad
                });

                if (window) {
                    window.show();

                    window.down('#createbutton').hide();
                    window.down('#updatebutton').hide();
                    window.down('#deletebutton').hide();
                    window.down('#historybutton').hide();
                    window.down('#deletedbutton').hide();
                    // необходимо отфильтровать записи в гриде, аналогично выпадающему списку в searchfield (выбираются технологии по связи Brand-Tecnology из справочника BrandTech)
                    var technologyIds = [],
                        technologyRecords = this.store.getRange(0, this.store.getCount());

                    technologyRecords.forEach(function (record) {
                        technologyIds.push(record.data.TechnologyId);
                    });
                    if (technologyIds.length === 0) {
                        window.close();
                        App.Notify.pushError(l10n.ns('tpm', 'ProductTree').value('ErrorNoBrandTech'));
                    } else {
                        technologyStore.setFixedFilter('Id', {
                            property: 'Id',
                            operation: 'In',
                            value: technologyIds
                        });
                    }
                }
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    if (newValue != null) {
                        var record = field.getStore().findRecord('TechnologyId', newValue);
                        field.up('editorform').down('textfield[name=Description_ru]').setValue(record.get('Technology_Description_ru'));
                    }
                }
            },
        }, {
            xtype: 'textfield',
            name: 'Name',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Description_ru')
        }, {
            xtype: 'textfield',
            name: 'Abbreviation',
            disabled: true,
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Abbreviation')
        }, {
            xtype: 'numberfield',
            name: 'NodePriority',
            disabled: true,
            fieldLabel: l10n.ns('tpm', 'ProductTree').value('NodePriority'),
            minValue: 0,
            allowBlank: false,
            allowOnlyWhitespace: false
        }]
    }]
});
