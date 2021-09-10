Ext.define('App.view.core.rpa.RPAEditor', {
	extend: 'App.view.core.common.EditorDetailWindow',
	alias: 'widget.rpaeditor',
	width: 500,
	minWidth: 500,
	maxHeight: 500,
	cls: 'readOnlyFields',

    noChange: true,

	items: {
		xtype: 'editorform',
		columnsCount: 1,
        items: [{
            xtype: 'container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'combobox',
                    name: 'HandlerName',
                    fieldLabel: l10n.ns('core', 'RPA').value('HandlerName'),
                    valueField: 'Name',
                    displayField: 'Name',           
                    entityType: 'RPASetting',
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    store: {
                        type: 'simplestore',
                        autoLoad: false,
                        model: 'App.model.core.rpasetting.RPASetting',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.core.rpasetting.RPASetting',
                                modelId: 'efselectionmodel'
                            }]
                         }
                    },
                    listeners: {
                        select: function(combo, record){
                            let paramFieldSet = Ext.getCmp('params');
                            if(paramFieldSet['items']['items'].length>0)
                            {
                                paramFieldSet.removeAll();
                            }
                            if(isJsonValid(record[0].data['Json'])){
                                const parametrs = JSON.parse(record[0].data['Json'])["parametrs"];
                                if(parametrs) {                           
                                        Ext.Array.each(parametrs,function(element,index){
                                            let paramField ={
                                                xtype: 'textfield',
                                                name: element["name"],
                                                fieldLabel: element["name"],
                                                value: element["value"],
                                                id: element["name"]
                                            };
                                            paramFieldSet.add(paramField);
                                        });
                                    }
                            }
                            else{
                                Ext.MessageBox.show({
                                    title: 'Error',
                                    msg: 'Wrong Json format. Please check RPA setting or leave parameters empty.',
                                    buttons: Ext.MessageBox.OK,
                                    icon: Ext.MessageBox.ERROR,
                                });
                            }
                        }
                    },
                    mapping: [{
                        from: 'Name',
                        to: 'HandlerName'
                    }]
                }, {
                    xtype: 'fieldset',
                    title: 'Parametrs',
                    fullscreen: true,
                    id: "params"
                }]
        }, {
            items: [{
            xtype: 'editorform',
            items: [{
                xtype: 'filefield',
                name: 'File',
                msgTarget: 'side',
                buttonText: l10n.ns('core', 'buttons').value('browse'),
                forceValidation: true,
                allowOnlyWhitespace: false,
                allowBlank: false,
                fieldLabel: l10n.ns('core').value('uploadFileLabelText'),
                vtype: 'filePass',
                ui: 'default',
                labelWidth: '10%'
                }]
            }]
        }]
    }    
});

function isJsonValid(str){
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}