# -*- coding: utf-8 -*-
from gluon.contrib.appconfig import AppConfig

myconf = AppConfig(reload=True)

db = DAL(myconf.take('db.uri'), pool_size=myconf.take('db.pool_size', cast=int), check_reserved=['all'])
dboee = DAL('sqlite://oee.db', pool_size=0, migrate=False)

response.generic_patterns = ['*'] if request.is_local else []

## choose a style for forms
response.formstyle = myconf.take('forms.formstyle')  # or 'bootstrap3_stacked' or 'bootstrap2' or other
response.form_label_separator = myconf.take('forms.separator')

from gluon.tools import Auth, Service, PluginManager

service = Service()
plugins = PluginManager()

## configure email
#mail = auth.settings.mailer
#mail.settings.server = 'logging' if request.is_local else myconf.take('smtp.server')
#mail.settings.sender = myconf.take('smtp.sender')
#mail.settings.login = myconf.take('smtp.login')

## Add colorwidget
import uuid
colorpicker_js = URL(r=request,c='static/mColorPicker', f='mColorPicker.min.js')
class ColorPickerWidget(object):
    """
    Colorpicker widget based on  http://code.google.com/p/mcolorpicker/
    """ 
    def __init__ (self, js = colorpicker_js, button=True, style="", transparency=False):
        uid = str(uuid.uuid4())[:8]
        self._class = "_%s" % uid
        self.style = style
        if transparency == False:
            self.transparency = 'false'
        else:
            self.transparency = 'true'
        if button == True:
            self.data = 'hidden'
            if self.style ==  "":
                self.style = "height:28px;width:28px;"
        else:
            self.data = 'display'
        if not js in response.files:
            response.files.append(js)
    def widget(self, f, v):
        wrapper = DIV()
        inp = SQLFORM.widgets.string.widget(f,v, _value=v, _type='color',\
            _data_text='hidden', _style=self.style, _hex='true', _class=self._class)
        scr = SCRIPT("$.fn.mColorPicker.init.replace = false;  \
             $.fn.mColorPicker.init.allowTransparency=%s; \
             $('input.%s').mColorPicker({'imageFolder': '/%s/static/mColorPicker/'});"\
              % (self.transparency, self._class, request.application))
        wrapper.components.append(inp)
        wrapper.components.append(scr)
        return wrapper

color_widget = ColorPickerWidget()

##Defined OEE tables
dboee.define_table('tblOee_Country', \
                   Field('fldOeeCountryTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryNr', 'integer', label='Country nr', readable=True, writable=False), \
                   Field('fldOeeCountryDescription', 'string', label='Country description'), \
                   Field('fldOeeCountryInformation', 'string', label='Country information'), \
                   Field('fldOeeCountryLanguageID', 'integer', label='Language ID', readable=False, writable=False), \
                   Field('fldOeeCountryHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.define_table('tblOee_Plant', \
                   Field('fldOeePlantTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantNr', 'integer', label='Plant nr', readable=True, writable=False), \
                   Field('fldOeePlantDescription', 'string', label='Plant description'), \
                   Field('fldOeePlantInformation', 'string', label='Plant information'), \
                   Field('fldOeePlantHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Plant.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')

dboee.define_table('tblOee_SubPlant', \
                   Field('fldOeeSubPlantTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantNr', 'integer', label='Sub-Plant nr', readable=True, writable=False), \
                   Field('fldOeeSubPlantDescription', 'string', label='Sub-Plant description'), \
                   Field('fldOeeSubPlantInformation', 'string', label='Sub-Plant information'), \
                   Field('fldOeeSubPlantHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_SubPlant.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_SubPlant.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')

dboee.define_table('tblOee_Department', \
                   Field('fldOeeDepartmentTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentNr', 'integer', label='Department nr', readable=True, writable=False), \
                   Field('fldOeeDepartmentDescription', 'string', label='Department department'), \
                   Field('fldOeeDepartmentInformation', 'string', label='Department information'), \
                   Field('fldOeeDepartmentHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Department.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Department.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Department.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')

dboee.define_table('tblOee_ActivityGroup', \
                   Field('fldOeeActivityGroupTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeActivityGroupNr', 'integer', label='Activitygroup', readable=True, writable=False), \
                   Field('fldOeeActivityGroupDescription', 'text', label='Activitygroup description'), \
                   Field('fldOeeActivityGroupInformation', 'text', label='Activitygroup information'), \
                   Field('fldOeeActivityGroupColorNr', 'integer', label='Activitygroup color'), \
                   Field('fldOeeActivityGroupCalcForOee', 'integer', label='Calculate OEE'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_ActivityGroup.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_ActivityGroup.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_ActivityGroup.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_ActivityGroup.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_Activity', \
                   Field('fldOeeActivityTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeActivityNr', 'integer', label='Activity nr', readable=True, writable=False), \
                   Field('fldOeeActivityGroupID', 'integer', label='Activitygroup'), \
                   Field('fldOeeActivityDescription', 'string', label='Activity description'), \
                   Field('fldOeeActivityInformation', 'string', label='Activity information'), \
                   Field('fldOeeActivityHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Activity.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Activity.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Activity.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_Activity.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
dboee.tblOee_Activity.fldOeeActivityGroupID.requires = IS_IN_DB(dboee(), dboee.tblOee_ActivityGroup.fldOeeActivityGroupNr, '%(fldOeeActivityGroupDescription)s')

dboee.define_table('tblOee_ModuleSensorStyle', \
                   Field('fldOeeModuleSensorStyleTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeeModuleSensorStyleNr', 'integer', label='Sensor-style nr', readable=True, writable=False), \
                   Field('fldOeeModuleSensorStyleDescription', 'string', label='Sensor-style'), \
                   Field('fldOeeModuleSensorStyleInformation', 'string', label='Sensor-style information'), \
                   Field('fldOeeModuleSensorStyleHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.define_table('tblOee_ModuleType', \
                   Field('fldOeeModuleTypeTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeModuleTypeNr', 'integer', label='Module-type nr'), \
                   Field('fldOeeModuleTypeConnection', 'string', label='Connection-type'), \
                   Field('fldOeeModuleTypeDescription', 'string', label='Module-type description'), \
                   Field('fldOeeModuleTypeInformation', 'string', label='Connection-type information'), \
                   Field('fldOeeModuleTypeHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.define_table('tblOee_Module', \
                   Field('fldOeeModuleTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeModuleNr', 'integer', label='Module nr', readable=True, writable=False), \
                   Field('fldOeeModuleTypeID', 'integer', label='Module-type'), \
                   Field('fldOeeModuleSensorStyleID', 'integer', label='Sensor-style'), \
                   Field('fldOeeModuleDescription', 'string', label='Module description'), \
                   Field('fldOeeModuleInformation', 'string', label='Module information'), \
                   Field('fldOeeModuleSensorAddress', 'integer', label='Sensor address'), \
                   Field('fldOeeModuleSensorResetAddress', 'integer', label='Sensor reset address'), \
                   Field('fldOeeModuleIpAddress', 'string', label='IP address'), \
                   Field('fldOeeModuleIpAddressPort', 'integer', label='IP Port'), \
                   Field('fldOeeModuleComPort', 'string', label='Com port'), \
                   Field('fldOeeModuleBitsPerSecond', 'integer', label='Bits per Second'), \
                   Field('fldOeeModuleDatabits', 'integer', label='Databits'), \
                   Field('fldOeeModuleStopBits', 'integer', label='StopBits'), \
                   Field('fldOeeModuleFlowControl', 'string', label='Flowcontrol'), \
                   Field('fldOeeModuleParity', 'string', label='Parity'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeModuleHistory', 'boolean', label='History'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Module.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Module.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Module.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_Module.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
dboee.tblOee_Module.fldOeeModuleSensorStyleID.requires = IS_IN_DB(dboee(), dboee.tblOee_ModuleSensorStyle.fldOeeModuleSensorStyleNr, '%(fldOeeModuleSensorStyleDescription)s')
dboee.tblOee_Module.fldOeeModuleTypeID.requires = IS_IN_DB(dboee(), dboee.tblOee_ModuleType.fldOeeModuleTypeNr, '%(fldOeeModuleTypeDescription)s')

dboee.define_table('tblOee_MachineIOFailure', \
                   Field('fldOeeMachineIOFailureTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineIOFailureNr', 'integer', label='I/O failure nr'), \
                   Field('fldOeeMachineIOFailureDescription', 'string', label='I/O failure'), \
                   Field('fldOeeMachineIOFailureInformation', 'string', label='I/O failure information'), \
                   Field('fldOeeMachineIOFailureHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineIOFailure.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineIOFailure.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineIOFailure.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineIOFailure.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_MachineShortbreak', \
                   Field('fldOeeMachineShortBreakTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineShortBreakNr', 'integer', label='Shortbreak nr', readable=True, writable=False), \
                   Field('fldOeeMachineShortBreakDescription', 'string', label='Shortbreak description'), \
                   Field('fldOeeMachineShortBreakInformation', 'string', label='Shortbreak information'), \
                   Field('fldOeeMachineShortBreakHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineShortbreak.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineShortbreak.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineShortbreak.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineShortbreak.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_MachineStatus', \
                   Field('fldOeeMachineStatusTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineStatusNr', 'integer', label='Machine status nr'), \
                   Field('fldOeeMachineStatusDescription', 'string', label='Machine status description'), \
                   Field('fldOeeMachineStatusInformation', 'string', label='Machine status information'), \
                   Field('fldOeeMachineStatusHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineStatus.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineStatus.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineStatus.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineStatus.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_MachineUndefinedProduction', \
                   Field('fldOeeMachineUndefinedProductionTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineUndefinedProductionNr', 'integer', label='Undefined Production nr', readable=True, writable=False), \
                   Field('fldOeeMachineUndefinedProductionDescription', 'string', label='Undefined Production description'), \
                   Field('fldOeeMachineUndefinedProductionInformation', 'string', label='Undefined Production information'), \
                   Field('fldOeeMachineUndefinedProductionHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineUndefinedProduction.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineUndefinedProduction.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineUndefinedProduction.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineUndefinedProduction.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_MachineUndefinedStandstill', \
                   Field('fldOeeMachineUndefinedStandstillTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineUndefinedStandstillNr', 'integer', label='Undefined standstill nr', readable=True, writable=False), \
                   Field('fldOeeMachineUndefinedStandstillDescription', 'string', label='Undefined standstill description'), \
                   Field('fldOeeMachineUndefinedStandstillInformation', 'string', label='Undefined standstill information'), \
                   Field('fldOeeMachineUndefinedStandstillHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineUndefinedStandstill.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineUndefinedStandstill.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineUndefinedStandstill.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineUndefinedStandstill.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_MachineUnscheduled', \
                   Field('fldOeeMachineUnscheduledTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineUnscheduledNr', 'integer', label='Unscheduled nr', readable=True, writable=False), \
                   Field('fldOeeMachineUnscheduledDescription', 'string', label='Unscheduled description'), \
                   Field('fldOeeMachineUnscheduledInformation', 'string', label='Unscheduled information'), \
                   Field('fldDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeMachineUnscheduledHistory', 'boolean', label='History'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineUnscheduled.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineUnscheduled.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineUnscheduled.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineUnscheduled.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_MachineUnit', \
                   Field('fldOeeMachineUnitTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineUnitNr', 'integer', label='Machine-unit nr'), \
                   Field('fldOeeMachineUnitDescription', 'string', label='Machine-unit description'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeMachineUnitHistory', 'boolean', label='History'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineUnit.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineUnit.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineUnit.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineUnit.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_Machine', \
                   Field('fldOeeMachineTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineNr', 'integer', label='Machine nr', readable=True, writable=False), \
                   Field('fldOeeMachineCode', 'integer', label='Machine code'), \
                   Field('fldOeeMachineDescription', 'string', label='Machine description'), \
                   Field('fldOeeMachineInformation', 'string', label='Machine information'), \
                   Field('fldOeeModuleID', 'integer', label='Module'), \
                   Field('fldOeeMachineShortBreakID', 'integer', label='Shortbreak'), \
                   Field('fldOeeMachineUndefinedProdID', 'integer', label='Undefined Production'), \
                   Field('fldOeeMachineUndefinedStandStillID', 'integer', label='Undefined Standstill'), \
                   Field('fldOeeMachineUnscheduledID', 'integer', label='Unscheduled'), \
                   Field('fldOeeMachineIOFailureID', 'integer', label='I/O failure'), \
                   Field('fldOeeMachineUnitID', 'integer', label='Machine-unit'), \
                   Field('fldOeeMachineSortOrder', 'integer', label='Machine sort order'), \
                   Field('fldOeeMachineProductionBoundaryTimer', 'integer', label='Production timer (sec)'), \
                   Field('fldOeeMachineProductionShortbreakTimer', 'integer', label='Shortbreak timer (sec)'), \
                   Field('fldOeeMachineStopCodeTimer', 'integer', label='Stopcode timer (sec)'), \
                   Field('fldOeeMachineSpeed', 'integer', label='Default speed (per min)'), \
                   Field('fldOeeMachineDevider', 'decimal(2,2)', label='Pulse factor', default=1), \
                   Field('fldOeeMachineOperatorFactor', 'decimal(2,2)', label='Operator factor', default=1), \
                   Field('fldOeeMachineTarget1OEE', 'integer', label='OEE target 1'), \
                   Field('fldOeeMachineTarget2OEE', 'integer', label='OEE target 2'), \
                   Field('fldOeeMachineWorkstationDescription', 'string', label='Workstation description'), \
                   Field('fldOeeMachineHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Machine.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Machine.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Machine.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_Machine.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
dboee.tblOee_Machine.fldOeeModuleID.requires = IS_IN_DB(dboee(), dboee.tblOee_Module.fldOeeModuleNr, '%(fldOeeModuleDescription)s')
dboee.tblOee_Machine.fldOeeMachineShortBreakID.requires = IS_IN_DB(dboee(), dboee.tblOee_MachineShortbreak.fldOeeMachineShortBreakNr, '%(fldOeeMachineShortBreakDescription)s')
dboee.tblOee_Machine.fldOeeMachineUndefinedProdID.requires = IS_IN_DB(dboee(), dboee.tblOee_MachineUndefinedProduction.fldOeeMachineUndefinedProductionNr, '%(fldOeeMachineUndefinedProductionDescription)s')
dboee.tblOee_Machine.fldOeeMachineUndefinedStandStillID.requires = IS_IN_DB(dboee(), dboee.tblOee_MachineUndefinedStandstill.fldOeeMachineUndefinedStandstillNr, '%(fldOeeMachineUndefinedStandstillDescription)s')
dboee.tblOee_Machine.fldOeeMachineUnscheduledID.requires = IS_IN_DB(dboee(), dboee.tblOee_MachineUnscheduled.fldOeeMachineUnscheduledNr, '%(fldOeeMachineUnscheduledDescription)s')
dboee.tblOee_Machine.fldOeeMachineIOFailureID.requires = IS_IN_DB(dboee(), dboee.tblOee_MachineIOFailure.fldOeeMachineIOFailureNr, '%(fldOeeMachineIOFailureDescription)s')
dboee.tblOee_Machine.fldOeeMachineUnitID.requires = IS_IN_DB(dboee(), dboee.tblOee_MachineUnit.fldOeeMachineUnitNr, '%(fldOeeMachineUnitDescription)s')

dboee.define_table('tblOee_MachineActivity', \
                   Field('fldOeeMachineActivityTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeMachineID', 'integer', label='Machine'), \
                   Field('fldOeeMachineActivityID', 'integer', label='Activity'), \
                   Field('fldOeeMachineActivitySortOrder', 'integer', label='Sort order'), \
                   Field('fldOeeMachineActivityHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_MachineActivity.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_MachineActivity.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_MachineActivity.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_MachineActivity.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
dboee.tblOee_MachineActivity.fldOeeMachineActivityID.requires = IS_IN_DB(dboee(), dboee.tblOee_Activity.fldOeeActivityNr, '%(fldOeeActivityDescription)s')
dboee.tblOee_MachineActivity.fldOeeMachineID.requires = IS_IN_DB(dboee(), dboee.tblOee_Machine.fldOeeMachineNr, '%(fldOeeMachineDescription)s')

dboee.define_table('tblOee_DailySchedule', \
                   Field('fldOeeDailyScheduleTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeDailyScheduleNr', 'integer', label='Daily schedule nr', readable=True, writable=False), \
                   Field('fldOeeTeamID', 'integer', label='Team'), \
                   Field('fldOeeShiftTimeID', 'integer', label='Shifttime'), \
                   Field('fldOeeDailyScheduleDescription', 'string', label='Daily schedule description'), \
                   Field('fldOeeDailyScheduleInformation', 'string', label='Daily schedule information'), \
                   Field('fldOeeDailyScheduleStartDate', 'datetime', label='Starttime'), \
                   Field('fldOeeDailyScheduleEndDate', 'datetime', label='Endtime'), \
                   Field('fldOeeDailyScheduleHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_DailySchedule.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')

dboee.define_table('tblOee_Article', \
                   Field('fldOeeArticleTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeArticleNr', 'string', label='Article nr'), \
                   Field('fldOeeArticleDescription', 'string', label='Article description'), \
                   Field('fldOeeArticleInformation', 'string', label='Article information'), \
                   Field('fldOeeArticleNormSpeed', 'integer', label='Norm speed'), \
                   Field('fldOeeArticleHistory', 'boolean', label='History', default = False), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Article.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Article.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Article.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_Article.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_Order', \
                   Field('fldOeeOrderTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeArticleID', 'string', label='Article'), \
                   Field('fldOeeOrderNr', 'string', label='Order nr'), \
                   Field('fldOeeOrderDescription', 'string', label='Order description'), \
                   Field('fldOeeOrderInformation', 'string', label='Order information'), \
                   Field('fldOeeOrderHistory', 'boolean', label='History', default = False), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Order.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Order.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Order.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_Order.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
dboee.tblOee_Order.fldOeeArticleID.requires = IS_IN_DB(dboee(), dboee.tblOee_Article.fldOeeArticleNr)

dboee.define_table('tblOee_ShiftTime', \
                   Field('fldOeeShiftTimeTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeShiftTimeNr', 'integer', label='Shifttime nr', readable=True, writable=False), \
                   Field('fldOeeShiftTimeDescription', 'string', label='Shifttime description'), \
                   Field('fldOeeShiftTimeInformation', 'string', label='Shifttime information'), \
                   Field('fldOeeShiftTimeStart', 'datetime', label='Starttime'), \
                   Field('fldOeeShiftTimeEnd', 'datetime', label='Endtime'), \
                   Field('fldOeeShiftTimeHistory', 'boolean', label='History'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_ShiftTime.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_ShiftTime.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_ShiftTime.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_ShiftTime.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_Team', \
                   Field('fldOeeTeamTableKeyID', 'id', readable=False), \
                   Field('fldOeeCountryID', 'integer', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department'), \
                   Field('fldOeeTeamNr', 'integer', label='Team nr', readable=True, writable=False), \
                   Field('fldOeeTeamDescription', 'string', label='Team description'), \
                   Field('fldOeeTeamInformation', 'string', label='Team information'), \
                   Field('fldOeeTeamColorNr', 'integer', label='Team Color'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified', default = request.now), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.tblOee_Team.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
dboee.tblOee_Team.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
dboee.tblOee_Team.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
dboee.tblOee_Team.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')

dboee.define_table('tblOee_Reg', \
                   Field('fldOeeRegTableKeyID', 'id', readable=False), \
                   Field('fldOeeRegNr', 'integer', label='Reg nr'), \
                   Field('fldOeeMachineCode', 'integer', label='Machine code'), \
                   Field('fldOeeMachineID', 'integer', label='Machine ID'), \
                   Field('fldOeeMachineDescription', 'string', label='Machine'), \
                   Field('fldOeeMachineStatusID', 'integer', label='Machine status ID'), \
                   Field('fldOeeMachineStatusDescription', 'string', label='Machine status description'), \
                   Field('fldOeeCountryID', 'integer', label='Country ID'), \
                   Field('fldOeeCountryDescription', 'string', label='Country'), \
                   Field('fldOeePlantID', 'integer', label='Plant ID'), \
                   Field('fldOeePlantDescription', 'string', label='Plant'), \
                   Field('fldOeeSubPlantID', 'integer', label='Sub-Plant ID'), \
                   Field('fldOeeSubPlantDescription', 'string', label='Sub-Plant'), \
                   Field('fldOeeDepartmentID', 'integer', label='Department ID'), \
                   Field('fldOeeDepartmentDescription', 'string', label='Department'), \
                   Field('fldOeeStartDateTime', 'datetime', label='Start date'), \
                   Field('fldOeeEndDateTime', 'datetime', label='End date'), \
                   Field('fldOeeActivityDuration', 'integer', label='Duration in sec.'), \
                   Field('fldOeeTeamID', 'integer', label='Team ID'), \
                   Field('fldOeeTeamDescription', 'string', label='Team'), \
                   Field('fldOeeTeamColorID', 'integer', label='Team color ID'), \
                   Field('fldOeeTeamColorDescription', 'string', label='Team color'), \
                   Field('fldOeeShiftTimeID', 'integer', label='Shift ID'), \
                   Field('fldOeeShiftTimeDescription', 'string', label='Shift'), \
                   Field('fldOeeShiftStartDateTime', 'datetime', label='Shift starttime'), \
                   Field('fldOeeShiftEndDateTime', 'datetime', label='Shift endtime'), \
                   Field('fldOeeShiftDuration', 'integer', label='Shift duration'), \
                   Field('fldOeeAverageSpeed', 'integer', label='Average speed'), \
                   Field('fldOeeNormSpeed', 'integer', label='Norm speed'), \
                   Field('fldOeeCounter', 'integer', label='Counter'), \
                   Field('fldOeeCounterUnitID', 'integer', label='Counter-unit ID'), \
                   Field('fldOeeCounterUnitDescription', 'string', label='Counter-unit'), \
                   Field('fldOeeActivityGroupID', 'integer', label='Activitygroup ID'), \
                   Field('fldOeeActivityGroupDescription', 'string', label='Activitygroup'), \
                   Field('fldOeeActivityID', 'integer', label='Activity ID'), \
                   Field('fldOeeActivityDescription', 'string', label='Activity'), \
                   Field('fldOeeArticleNr', 'string', label='Article nr'), \
                   Field('fldOeeArticleDescription', 'string', label='Article description'), \
                   Field('fldOeeOrderNr', 'string', label='Order nr'), \
                   Field('fldOeeOrderDescription', 'string', label='Order description'), \
                   Field('fldOeeUserLogInformation', 'string', label='Activity log'), \
                   Field('fldOeeUserShiftLogInformation', 'string', label='Shift log'), \
                   Field('fldOeeCurrentPerformance', 'integer', label='Performance'), \
                   Field('fldOeeCurrentAvailability', 'integer', label='Availability'), \
                   Field('fldOeeCurrentQuality', 'integer', label='Quality'), \
                   Field('fldOeeCurrentOee', 'integer', label='OEE'), \
                   Field('fldOeeActivityGroupCalcForOee', 'integer', label='Calculate for OEE'), \
                   Field('fldOeeDateModified', 'datetime', label='Date modified'), \
                   Field('fldOeeSyncDate', 'datetime', label='Sync date'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.define_table('tblOee_Progress', \
                   Field('fldOeeProgressTableKeyID', 'id'), \
                   Field('fldOeeRegID', 'integer'), \
                   Field('fldOeeStartDateTime', 'datetime'), \
                   Field('fldOeeActivityDuration', 'integer'), \
                   Field('fldOeeCounter', 'integer'), \
                   Field('fldOeeNormSpeed', 'integer'), \
                   Field('fldOeeCountryID', 'integer'), \
                   Field('fldOeePlantID', 'integer'), \
                   Field('fldOeeSubPlantID',  'integer'), \
                   Field('fldOeeDepartmentID', 'integer'), \
                   Field('fldOeeCurrentOee', 'integer'), \
                   Field('fldOeeCurrentAvailability', 'integer'), \
                   Field('fldOeeCurrentPerformance', 'integer'), \
                   Field('fldOeeCurrentQuality', 'integer'), \
                   Field('fldOeeRegHistory', 'boolean'), \
                   Field('fldOeeDateModified', 'datetime'), \
                   Field('fldOeeMachineID', 'integer'), \
                   Field('fldOeeSyncDate', 'datetime'), \
                   Field('fldOeeSync', 'boolean', label='Sync', default = True))

dboee.define_table('tblOee_UserRight', \
                  Field('fldOeeUserRightTableKeyID', 'id'), \
                  Field('fldOeeUserRightNr', 'integer'), \
                  Field('fldOeeUserRightDescription', 'string'), \
                  Field('fldDateModified', 'datetime'), \
                  Field('fldOeeUserRightInformation', 'string'), \
                  Field('fldOeeUserRightHistory', 'boolean'))

## configure auth policy
auth = Auth(dboee)
auth.settings.table_user_name = 'tblOee_User'
auth.settings.extra_fields['tblOee_User']= [
    Field('fldOeeCountryID', 'integer', label='CountryID'), \
    Field('fldOeePlantID', 'integer', label='PlantID'), \
    Field('fldOeeSubPlantID', 'integer', label='SubPlantID'), \
    Field('fldOeeDepartmentID', 'integer', label='DepartmentID'), \
    Field('fldOeeUserRightID', 'integer', label='UserRightID'), \
    Field('fldOeeUserDescription', 'string', label='User description'), \
    Field('fldOeeUserLogin', 'string', label='UserLogin'), \
    Field('fldOeeUserDomain', 'string', label='Domain'), \
    Field('fldOeeDateModified', 'datetime'), \
    Field('fldOeeUserHistory', 'boolean')]

auth.define_tables(username=False, signature=False)
#auth.settings.actions_disabled.append('register')
auth.settings.registration_requires_verification = False
auth.settings.registration_requires_approval = False
auth.settings.reset_password_requires_verification = True
auth.settings.allow_basic_login = True

custom_auth_table = dboee[auth.settings.table_user_name]
custom_auth_table.fldOeeCountryID.requires = IS_IN_DB(dboee(), dboee.tblOee_Country.fldOeeCountryNr, '%(fldOeeCountryDescription)s')
custom_auth_table.fldOeePlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_Plant.fldOeePlantNr, '%(fldOeePlantDescription)s')
custom_auth_table.fldOeeSubPlantID.requires = IS_IN_DB(dboee(), dboee.tblOee_SubPlant.fldOeeSubPlantNr, '%(fldOeeSubPlantDescription)s')
custom_auth_table.fldOeeDepartmentID.requires = IS_IN_DB(dboee(), dboee.tblOee_Department.fldOeeDepartmentNr, '%(fldOeeDepartmentDescription)s')
custom_auth_table.fldOeeUserRightID.requires = IS_IN_DB(dboee(), dboee.tblOee_UserRight.fldOeeUserRightNr, '%(fldOeeUserRightDescription)s')
