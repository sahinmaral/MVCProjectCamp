using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer.Concrete
{
    public class HeadingManager : IHeadingService
    {
        private IHeadingDal _headingDal;
        public HeadingManager(IHeadingDal headingDal)
        {
            _headingDal = headingDal;
        }

        public Heading Get(Expression<Func<Heading, bool>> filter)
        {
            return  _headingDal.Get(filter);
        }

        public Heading GetById(int id)
        {
            return _headingDal.Get(x => x.HeadingId == id);
        }

        public int GetCount()
        {
            return _headingDal.List().Count();
        }

        public int GetCount(Expression<Func<Heading, bool>> filter)
        {
            return _headingDal.List(filter).Count();
        }

        public List<Heading> GetList()
        {
            return _headingDal.List();
        }

        public List<Heading> GetList(Expression<Func<Heading, bool>> filter)
        {
            return _headingDal.List(filter);
        }

        private string TurkishCharacterToEnglish(string text)
        {
            char[] turkishChars = { 'ı', 'ğ', 'İ', 'Ğ', 'ç', 'Ç', 'ş', 'Ş', 'ö', 'Ö', 'ü', 'Ü' };
            char[] englishChars = { 'i', 'g', 'I', 'G', 'c', 'C', 's', 'S', 'o', 'O', 'u', 'U' };

            // Match chars
            for (int i = 0; i < turkishChars.Length; i++)
                text = text.Replace(turkishChars[i], englishChars[i]);

            return text;
        }

        private string ToUrlSlug(string value)
        {
            // Make the string lower case.  
            string output = value.ToLower();

            // Convert Turkish character to English
            output = TurkishCharacterToEnglish(output);

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }

        public void Add(Heading heading)
        {
            heading.HeadingDate = DateTime.Now;
            heading.HeadingNameForFriendlyUrl = ToUrlSlug(heading.HeadingName);
            _headingDal.Insert(heading);
        }

        public void Delete(Heading heading)
        {
            
            _headingDal.Update(heading);
        }

        public void Update(Heading heading)
        {
            _headingDal.Update(heading);
        }

    }
}
