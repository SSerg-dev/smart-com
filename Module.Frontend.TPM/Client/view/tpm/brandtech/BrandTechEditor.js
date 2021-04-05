Ext.define('App.view.tpm.brandtech.BrandTechEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.brandtecheditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
            name: 'BrandId',
            selectorWidget: 'brand',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
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
            onTrigger2Click: function (){
                var brandtech_code = this.up('editorform').down('[name=BrandTech_code]');
                var brandsegtechsub_code = this.up('editorform').down('[name=BrandsegTechsub_code]');

                this.clearValue();
                brandtech_code.setValue(null);
                brandsegtechsub_code.setValue(null);
            },
            onSelectButtonClick: function (button) {
                var picker = this.picker,
                    selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
                    record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

                this.setValue(record);
                if (this.needUpdateMappings) {
                    this.updateMappingValues(record);
                }
                this.afterSetValue(record);

                if (this.up('editorform').down('[name=TechnologyId]').rawValue != null && this.up('editorform').down('[name=TechnologyId]').rawValue != '') {
                    var tech = this.up('editorform').down('[name=TechnologyId]');
                    var brandtech_code = this.up('editorform').down('[name=BrandTech_code]');
                    var brandsegtechsub_code = this.up('editorform').down('[name=BrandsegTechsub_code]');

                    tech.getStore().load(function(){
                        var tech_code = tech.getStore().getById(tech.value).data.Tech_code;
                        var sub_code = tech.getStore().getById(tech.value).data.SubBrand_code;

                        var brandtech_codeValue = record != undefined ?
                            record.get('Brand_code') + '-' + record.get('Segmen_code') + '-' + tech_code
                            : brandtech_code.rawValue;
                        var brandsegtechsub_codeValue = record != undefined ?
                            (sub_code == null || sub_code == '' ?
                                record.get('Brand_code') + '-' + record.get('Segmen_code') + '-' + tech_code
                                : record.get('Brand_code') + '-' + record.get('Segmen_code') + '-' + tech_code + '-' + sub_code)
                            : brandsegtechsub_code.rawValue;

                        var brandtechCodeValue = brandtech_codeValue;
                        var brandsegtechsubCodeValue = brandsegtechsub_codeValue;

                        brandtech_code.setValue(brandtechCodeValue);
                        brandsegtechsub_code.setValue(brandsegtechsubCodeValue);
                    });                   
                }               

                picker.close();
            },
            mapping: [{
                from: 'Name',
                to: 'BrandName'
            }]
        },
        {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyId',
            selectorWidget: 'technology',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.technology.Technology',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.technology.Technology',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            onTrigger2Click: function() {
                var sub = this.up('editorform').down('[name=SubBrandName]');
                var brandtech_code = this.up('editorform').down('[name=BrandTech_code]');
                var brandsegtechsub_code = this.up('editorform').down('[name=BrandsegTechsub_code]');

                this.clearValue();
                sub.setValue(null);
                brandtech_code.setValue(null);
                brandsegtechsub_code.setValue(null);
            },
            onSelectButtonClick: function (button) {
                var picker = this.picker,
                    selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
                    record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

                this.setValue(record);
                if (this.needUpdateMappings) {
                    this.updateMappingValues(record);
                }
                this.afterSetValue(record);

                if (this.up('editorform').down('[name=BrandId]').rawValue != null && this.up('editorform').down('[name=BrandId]').rawValue != '') {
                    var brand = this.up('editorform').down('[name=BrandId]');
                    var desc = this.up('editorform').down('[name=Technology_Description_ru]');
                    var sub = this.up('editorform').down('[name=SubBrandName]');
                    var brandtech_code = this.up('editorform').down('[name=BrandTech_code]');
                    var brandsegtechsub_code = this.up('editorform').down('[name=BrandsegTechsub_code]');

                    brand.getStore().load(function () {
                        var brand_code = brand.getStore().getById(brand.value).data.Brand_code;
                        var seg_code = brand.getStore().getById(brand.value).data.Segmen_code;

                        var brandtech_codeValue = record != undefined ?
                            brand_code + '-' + seg_code + '-' + record.get('Tech_code')
                            : brandtech_code.rawValue;
                        var brandsegtechsub_codeValue = record != undefined ?
                            (record.get('SubBrand_code') == null || record.get('SubBrand_code') == '' ?
                                brand_code + '-' + seg_code + '-' + record.get('Tech_code')
                                : brand_code + '-' + seg_code + '-' + record.get('Tech_code') + '-' + record.get('SubBrand_code'))
                            : brandsegtechsub_code.rawValue;

                        var description = record != undefined ? record.get('Description_ru') : desc.rawValue;
                        var subValue = record != undefined ? record.get('SubBrand') : sub.rawValue;
                        var brandtechCodeValue = brandtech_codeValue;
                        var brandsegtechsubCodeValue = brandsegtechsub_codeValue;

                        desc.setValue(description);
                        sub.setValue(subValue);
                        brandtech_code.setValue(brandtechCodeValue);
                        brandsegtechsub_code.setValue(brandsegtechsubCodeValue);
                    });
                }           

                picker.close();
            },
            mapping: [{
                from: 'Name',
                to: 'TechnologyName'
            }]
        }, {
            xtype: 'textfield',
            name: 'Technology_Description_ru',
            fieldLabel: l10n.ns('tpm', 'Product').value('Technology_Description_ru'),
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrandName'),
            name: 'SubBrandName',
            valueField: 'SubBrand',
            displayField: 'SubBrand',
            readOnly: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.technology.Technology',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.technology.Technology',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            },
            mapping: [{
                from: 'SubBrand',
                to: 'SubBrandName'
            }]
        }, {
            xtype: 'textfield',
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandTech_code'),
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield',
            name: 'BrandsegTechsub_code',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandsegTechsub_code'),
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }]
    }
});