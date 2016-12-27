using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jqx
{
    public class jqxMaskedInput : jqxBase
    {
        /// <summary>
        /// Sets or gets the masked input's value. 
        /// </summary>
        public string value
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the masked input's mask. 
        /// Mask characters: •# - For digit character. Accepts values from 0 to 9 
        ///     9 - For digit character.Accepts values from 0 to 9 
        ///     0 - For digit character.Accepts values from 0 to 9 
        ///     A - For alpha numeric character.Accepts values from 0 to 9 and from a to z and A to Z.
        ///     L - For alpha character.Accepts values from a to z and A to Z 
        ///     [abcd] - For character set.Matches any one of the enclosed characters.You can specify a range of characters by using a hyphen.For example, [abcd] is the same as [a-d]. Examples: [0-5] - accepts values from 0 to 5. [ab] - accepts only a or b.
        /// </summary>
        public string mask
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the text alignment. 
        /// </summary>
        public horizontalAlignment textAlign
        {
            get { return _get<horizontalAlignment>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the readOnly state of the input. 
        /// </summary>
        public bool readOnly
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public bool cookies
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets or gets the prompt char displayed when an editable char is empty. 
        /// </summary>
        public string promptChar
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        public inputMode inputMode
        {
            get { return _get<inputMode>(); }
            set { _set(value); }
        }
    }
}