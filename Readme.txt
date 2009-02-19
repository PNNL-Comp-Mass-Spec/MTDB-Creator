The MTDBCreator will read a series of Peptide Hit Result Processor (PHRP) output files
from X!Tandem or Sequest search results, align the data, then create a standalone
Microsoft Access DB.  For an example please see the ExampleDatabase.mdb file
installed with the program (typically at C:\Program Files\MTDBCreator\)  For
more information on PHRP, see http://omics.pnl.gov/software/PeptideHitResultsProcessor.php

Use File->Open Dataset List to select a file defining the paths to the folders
containing the input PHRP files to use to create the mass tag database.  As an 
example, see file DatasetDescription.txt.  You will need to customize this file
as needed to read your own data files.  Use Tools->Options to define the filters
for aligning the data and for storing the data in the database.  Next, use
Tools->Create Mass Tag Database to align the data files and create the Microsoft
Access database.  Following this, the alignment of individual datasets can be 
visualized by clicking on the dataset name in the list at the bottom of the program

-------------------------------------------------------------------------------
Written by Deep Jaitly and Matthew Monroe for the Department of Energy (PNNL, Richland, WA)
Copyright 2007, Battelle Memorial Institute.  All Rights Reserved.

E-mail: matthew.monroe@pnl.gov or proteomics@pnl.gov
Website: http://ncrr.pnl.gov/ or http://www.sysbio.org/resources/staff/
-------------------------------------------------------------------------------

Licensed under the Apache License, Version 2.0; you may not use this file except 
in compliance with the License.  You may obtain a copy of the License at 
http://www.apache.org/licenses/LICENSE-2.0

All publications that result from the use of this software should include 
the following acknowledgment statement:
 Portions of this research were supported by the W.R. Wiley Environmental 
 Molecular Science Laboratory, a national scientific user facility sponsored 
 by the U.S. Department of Energy's Office of Biological and Environmental 
 Research and located at PNNL.  PNNL is operated by Battelle Memorial Institute 
 for the U.S. Department of Energy under contract DE-AC05-76RL0 1830.

Notice: This computer software was prepared by Battelle Memorial Institute, 
hereinafter the Contractor, under Contract No. DE-AC05-76RL0 1830 with the 
Department of Energy (DOE).  All rights in the computer software are reserved 
by DOE on behalf of the United States Government and the Contractor as 
provided in the Contract.  NEITHER THE GOVERNMENT NOR THE CONTRACTOR MAKES ANY 
WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS 
SOFTWARE.  This notice including this sentence must appear on any copies of 
this computer software.
