Ext.define("App.view.tpm.rpa.RPA", {
    override: "App.view.core.rpa.RPA",

    dockedItems: [{
        xtype: "readonlydirectorytoolbar",
        dock: "right",
        items: [{
            xtype: "widthexpandbutton",
            ui: "fill-gray-button-toolbar",
            text: l10n.ns("core", "selectablePanelButtons").value("toolbarCollapse"),
            glyph: 61757,
            glyph1: 61758,
            target: function() {
                return this.up("toolbar")
            }
        }, {
            itemId: "createbutton",
            action: "Post",
            glyph: 62485,
            text: l10n.ns("core", "crud").value("createButtonText"),
            tooltip: l10n.ns("core", "crud").value("createButtonText")
        }, {
            itemId: "updatebutton",
            action: "Patch",
            glyph: 63055,
            text: l10n.ns("core", "crud").value("updateButtonText"),
            tooltip: l10n.ns("core", "crud").value("updateButtonText")
        }, {
            itemId: "updategroupbutton",
            glyph: 62704,
            action: "Patch",
            text: l10n.ns("tpm", "button").value("updateGroupButtonText"),
            tooltip: l10n.ns("tpm", "button").value("updateGroupButtonText")
        }, {
            itemId: "deletebutton",
            action: "Delete",
            glyph: 62952,
            text: l10n.ns("core", "crud").value("deleteButtonText"),
            tooltip: l10n.ns("core", "crud").value("deleteButtonText")
        }, {
            itemId: "historybutton",
            resource: "Historical{0}",
            action: "Get{0}",
            glyph: 62170,
            text: l10n.ns("core", "crud").value("historyButtonText"),
            tooltip: l10n.ns("core", "crud").value("historyButtonText")
        }, "-", {
            itemId: "extfilterbutton",
            glyph: 62281,
            text: l10n.ns("core", "toptoolbar").value("filterButtonText"),
            tooltip: l10n.ns("core", "toptoolbar").value("filterButtonText")
        }, {
            itemId: "deletedbutton",
            resource: "Deleted{0}",
            action: "Get{0}",
            glyph: 62040,
            text: l10n.ns("core", "toptoolbar").value("deletedButtonText"),
            tooltip: l10n.ns("core", "toptoolbar").value("deletedButtonText")
        }, {
            itemId: 'showlogbutton',
            disabled: false,
            glyph: 0xF262,
            text: 'Show Log',
            tooltip: 'Show Log'
        }, "-", "->", "-", {
            itemId: "extfilterclearbutton",
            ui: "blue-button-toolbar",
            disabled: !0,
            glyph: 62002,
            text: l10n.ns("core", "filter").value("filterEmptyStatus"),
            tooltip: l10n.ns("core", "filter").value("filterEmptyStatus"),
            overCls: "",
            style: {
                cursor: "default"
            }
        }]
    }],
    customHeaderItems: [ResourceMgr.getAdditionalMenu("core").base],
    items: [{
        xtype: "directorygrid",
        itemId: "datatable",
        editorModel: "Core.form.EditorDetailWindowModel",
        store: {
            type: "directorystore",
            model: "App.model.tpm.rpa.RPA",
            storeId: "rpastore",
            sorters: [{
                property: "CreateDate",
                direction: "DESC"
            }],
            extendedFilter: {
                xclass: "App.ExtFilterContext",
                supportedModels: [{
                    xclass: "App.ExtSelectionFilterModel",
                    model: "App.model.tpm.rpa.RPA",
                    modelId: "efselectionmodel"
                }, {
                    xclass: "App.ExtTextFilterModel",
                    modelId: "eftextmodel"
                }]
            }
        },
        columns: {
            defaults: {
                plugins: ["sortbutton"],
                menuDisabled: !0,
                filter: !0,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns("core", "RPA").value("HandlerName"),
                dataIndex: "HandlerName"
            }, {
                text: l10n.ns("core", "RPA").value("CreateDate"),
                dataIndex: "CreateDate",
                xtype: "datecolumn",
                renderer: Ext.util.Format.dateRenderer("d.m.Y H:i:s")
            }, {
                text: l10n.ns("core", "RPA").value("UserName"),
                dataIndex: "UserName"
            }, {
                text: l10n.ns("core", "RPA").value("Constraint"),
                dataIndex: "Constraint"
            }, {
                text: l10n.ns("core", "RPA").value("Status"),
                dataIndex: "Status"
            }, {
                text: l10n.ns("core", "RPA").value("FileURL"),
                dataIndex: "FileURL",
                renderer: function(e) {
                    var t = document.location.href + Ext.String.format("/odata/RPAs/DownloadFile?fileName={0}", e || "");
                    return e = e ? "<a href=" + t + ">" + e + "</a>" : ""
                }
            }]
        }
    }, {
        xtype: "editabledetailform",
        itemId: "detailform",
        model: "App.model.tpm.rpa.RPA",
        items: [{
            xtype: "singlelinedisplayfield",
            name: "HandlerName",
            fieldLabel: l10n.ns("core", "RPA").value("HandlerName")
        }, {
            xtype: "singlelinedisplayfield",
            name: "CreateDate",
            renderer: Ext.util.Format.dateRenderer("d.m.Y H:i:s"),
            fieldLabel: l10n.ns("core", "RPA").value("CreateDate")
        }, {
            xtype: "textfield",
            name: "UserName",
            fieldLabel: l10n.ns("core", "RPA").value("UserName")
        }, {
            xtype: "textfield",
            name: "Constraint",
            fieldLabel: l10n.ns("core", "RPA").value("Constraint")
        }, {
            xtype: "textfield",
            name: "Status",
            fieldLabel: l10n.ns("core", "RPA").value("Status")
        }, {
            xtype: "textfield",
            name: "FileURL",
            fieldLabel: l10n.ns("core", "RPA").value("FileURL")
        }]
    }]
});