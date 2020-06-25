Ext.define('App.view.tpm.color.ColorEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.coloreditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    afterWindowShow: function () {
        this.down('textfield[name=DisplayName]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'circlecolorfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Color').value('SystemName'),
        }, {
            xtype: 'textfield',
            name: 'DisplayName',
            fieldLabel: l10n.ns('tpm', 'Color').value('DisplayName')
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            valueField: 'Id',
            displayField: 'BrandName',
            onTrigger2Click: function () {
                var technology = this.up().down('[name=TechnologyName]');

                this.clearValue();
                technology.setValue(null);
            },
            listeners: {
                change: function (field, newValue, oldValue) {
                    var technology = field.up('editorform').down('[name=TechnologyName]');
                    var techValue = newValue != undefined ? field.record.get('TechnologyName') : null;

                    var sub = field.up('editorform').down('[name=SubBrandName]');
                    var subValue = newValue != undefined ? field.record.get('SubBrandName') : null;

                    technology.setValue(techValue);
                    sub.setValue(subValue);
                }
            },
            store: {
                type: 'directorystore',
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
                from: 'BrandName',
                to: 'BrandName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrandName'),
            name: 'SubBrandName',
        }]
    }
});
