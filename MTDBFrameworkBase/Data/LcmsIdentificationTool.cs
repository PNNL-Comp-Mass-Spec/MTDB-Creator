﻿namespace MTDBFrameworkBase.Data
{
    /// <summary>
    /// Controlled vocabulary for LCMS formats
    /// </summary>
    public enum LcmsIdentificationTool
    {
        /// <summary>
        /// Sequest
        /// </summary>
        Sequest,

        /// <summary>
        /// X!Tandem
        /// </summary>
        XTandem,

        /// <summary>
        /// MSAlign
        /// </summary>
        MSAlign,

        /// <summary>
        /// MSGF+
        /// </summary>
        MsgfPlus,

        /// <summary>
        /// MZIdentML
        /// </summary>
        MZIdentML,

        /// <summary>
        /// RAW
        /// </summary>
        Raw,

        /// <summary>
        /// Description
        /// </summary>
        Description,

        /// <summary>
        /// Other, Unsupported type
        /// </summary>
        NOT_SUPPORTED
    }
}