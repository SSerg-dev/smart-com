Ext.define('App.view.tpm.rpa.RPAForm', {
    override: 'App.view.core.rpa.RPAForm',
    items: [{
        xtype: "editorform",
        columnsCount: 1,
        layout: {
            type: "vbox",
            align: "stretch"
        },
        scroll: true,
        overflowY: "scroll",
        height: "100%",
        items: [{
            xtype: "panel",
            height: "100%",
            autoScroll: true,
            flex: 1,
            layout: {
                type: "vbox",
                align: "stretch",
                pack: "start"
            },
            items: [{
                xtype: "fieldset",
                title: "Handlers and templates",
                items: [{
                    layout: {
                        type: "vbox",
                        align: "stretch"
                    },
                    xtype: "container",
                    items: [{
                        xtype: "combobox",
                        name: "HandlerName",
                        fieldLabel: l10n.ns("core", "RPA").value("HandlerName"),
                        valueField: "Name",
                        displayField: "Name",
                        entityType: "RPASetting",
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        store: {
                            type: "simplestore",
                            autoLoad: false,
                            model: "App.model.core.rpasetting.RPASetting",
                            extendedFilter: {
                                xclass: "App.ExtFilterContext",
                                supportedModels: [{
                                    xclass: "App.ExtSelectionFilterModel",
                                    model: "App.model.core.rpasetting.RPASetting",
                                    modelId: "efselectionmodel"
                                }]
                            }
                        },
                        listeners: {
                            select: function(t, o) {
                                let e = Ext.getCmp("templateLink");
                                let n, i;
                                Ext.get("importLink").clearListeners();
                                if (isJsonValid(o[0].data.Json)) {
                                    i = this.up("customrpaeditor");
                                    n = new Ext.LoadMask({
                                        msg: "Downloading...",
                                        target: i
                                    });
                                    e.setVisible(true);
                                    Ext.get("importLink").addListener("click", function() {
                                        n.show();
                                        var e = Ext.String.format("odata/{0}/{1}", "RPAs", "DownloadTemplateXLSX");
                                        Ext.Ajax.request({
                                            method: "POST",
                                            url: e,
                                            params: {
                                                handlerId: Ext.JSON.encode(o[0].data.Id)
                                            },
                                            success: function(e) {
                                                var e = JSON.parse(e.responseText).value;
                                                t = Ext.String.format("api/File/{0}?{1}={2}", "ExportDownload", "filename", e);
                                                o = document.createElement("a");
                                                o.download = e;
                                                o.href = t;
                                                document.body.appendChild(o);
                                                o.click();
                                                document.body.removeChild(o);
                                                n.hide()
                                            },
                                            failure: function(e) {
                                                App.Notify.pushError(Ext.JSON.decode(e.responseText)["odata.error"].innererror.message);
                                                e = t.up("customrpaeditor");
                                                e.setLoading(false);
                                                e.close()
                                            }
                                        })
                                    });
                                    Ext.getCmp('rpaType').setValue(JSON.parse(o[0].data['Json'])["type"]);
                                } else {
                                    Ext.MessageBox.show({
                                        title: "Error",
                                        msg: "Wrong Json format. Please check RPA setting or parameters is empty.",
                                        buttons: Ext.MessageBox.OK,
                                        icon: Ext.MessageBox.ERROR
                                    });
                                }
                            },
                            afterrender: function(t) {
                                (store = t.store).on("load", function(e) {
                                    0 === e.data.items.length && (t.setValue("This role does not have a configured handler"),
                                        Ext.getCmp("eventfile").setDisabled(true),
                                        Ext.ComponentQuery.query("#saveRPAForm")[0].setDisabled(true))
                                })
                            }
                        },
                        mapping: [{
                            from: "Name",
                            to: "HandlerName"
                        }]
                    }, {
                        xtype: "label",
                        glyph: 61981,
                        html: '<span id="importLink" style="cursor:pointer; color:blue; text-decoration:underline;">Import template XLSX</span>',
                        id: "templateLink",
                        hidden: true,
                        style: {
                            "text-align": "right"
                        }
                    }]
                }]
            }, {
                xtype: "filefield",
                name: "File",
                id: "eventfile",
                msgTarget: "side",
                buttonText: l10n.ns("core", "buttons").value("browse"),
                forceValidation: true,
                allowOnlyWhitespace: false,
                allowBlank: false,
                fieldLabel: l10n.ns("core").value("uploadFileLabelText"),
                vtype: "filePass",
                ui: "default",
                labelWidth: "10%"
            }, {
                xtype: "hiddenfield",
                name: "rpaType",
                id: "rpaType",
                value: ""
            }]
        }]
    }]
});