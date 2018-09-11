# MTDB Creator

The MTDB Creator can create a standalone Accurate Mass and Time Tag 
Database (AMT tag DB) compatible with MultiAlign or VIPER. The database
is assembled using peptide identifications (identified PSMs) from 
MS-GF+, X!Tandem, SEQUEST, or MSAlign.  It loads the PSMs, align the 
peptides across datasets, then writes the results to the database.

MTDB Creator can directly load the .mzid files created by MS-GF+. 
It can also load .mzid.gz files. To process X!Tandem results, .xml 
file created by X!Tandem must first be converted to a tab-delimited 
format using the Peptide Hit Results Processor application (PHRP); 
see https://omics.pnl.gov/software/peptide-hit-results-processor.

## Details

Start the MTDB Creator, then click "New Analysis Job". 
* Click "Add", select "MZIdentML", and select one or more .mzid or .mzid.gz files. 
* Alternatively, use "Add Folder" and select a directory with several .mzid or .mzid.gz files.

For X!Tandem, SEQUEST, or MSAlign results, use "Add" then "Old Analysis Files". 
* For X!Tandem, select the _xt.txt files created by PHRP. 
* For SEQUEST, select the _syn.txt files created by PHRP. 
* For MSAlign select _msalign_syn.txt files created by PHRP.

Next, click Options, review the filtering and alignment options, and
confirm the database format. 
* Click "OK, and the program will prompt you to define the output file path. 

It will next load the peptide identifications for each dataset, filter them, 
and align them against a predicted normalized elution time (NET) scale. 
* Once the processing finishes, the AMT tag database will be created, 
and a window appear with alignment plots.

## Example Data

Example data files are available in a 130 MB .zip file at 
https://omics.pnl.gov/sites/default/files/MTDB_Creator_Example_Data.zip
for you to explore MTDB Creator's functionality. Unzip that .Zip file to obtain these files:

1. MSGF+ results files to be used as input files for MTDB Creator
* QC_Shew_17-02_a_QEP_03Aug18_Wally_18-07-04_msgfplus.mzid.gz
* QC_Shew_17-02_b_QEP_03Aug18_Wally_18-07-04_msgfplus.mzid.gz
* QC_Shew_17-02_d_QEP_25July18_Wally_18-07-04_msgfplus.mzid.gz

2. VIPER compatible AMT tag DB created by MTDB Creator
* QC_Shew.mdb

3. MultiAlign compatible AMT tag DB created by MTDB Creator
* QC_Shew.mtdb

4. Deisotoped features files generated by DeconTools
* See the _isos.csv and _scans.csv files in DeconTools_Results

5. MSFileInfoScanner graphics files

The QC_Shew datasets were generated from the analysis of a 
Shewanella Oneidensis MR-1 QA sample, using a Thermo Q-Exactive Plus 
mass spectrometer.
	
## Contacts

Written by Matthew Monroe, Brian LaMarche, Michael Degan, and Bryson Gibbons for the Department of Energy (PNNL, Richland, WA) \
E-mail: matthew.monroe@pnnl.gov or proteomics@pnnl.gov \
Website: https://omics.pnl.gov/ or https://panomics.pnnl.gov/

## License

The MTDB Creator is licensed under the 2-Clause BSD License; 
you may not use this file except in compliance with the License.  You may obtain 
a copy of the License at https://opensource.org/licenses/BSD-2-Clause

Copyright 2018 Battelle Memorial Institute
