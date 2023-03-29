####Notebook "ML_PLAN_PARAMETERS_CALCULATION". 
####*Main night all parameters recalculation notebook. Get all parameters for promo, promoproduct*.
###### *Developer: [LLC Smart-Com](http://smartcom.software/), andrey.philushkin@effem.com*

def is_notebook() -> bool:
    try:
        shell = get_ipython().__class__.__name__
        if shell == 'ZMQInteractiveShell':
            return True   # Jupyter notebook or qtconsole
        elif shell == 'TerminalInteractiveShell':
            return False  # Terminal running IPython
        else:
            return False  # Other type (?)
    except NameError:
        return False      # Probably standard Python interpreter
        
if is_notebook():
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/ML_PLAN_PARAMETERS_CALCULATION.py")

import ML_PLAN_PARAMETERS_CALCULATION